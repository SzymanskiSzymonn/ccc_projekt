using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ccc_projekt.Data;
using ccc_projekt.Models;
using Microsoft.AspNetCore.Authorization;

namespace ccc_projekt.Controllers
{
    public class TasksController : Controller
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.Task
        .Include(t => t.User)       // Dołącz dane o użytkowniku
        .Include(t => t.Project)
        .OrderBy(t => t.AssignedProject)
        .ToListAsync();

            return View(tasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var tasks = await _context.Task
        .Include(t => t.User)       // Dołącz dane o użytkowniku
        .Include(t => t.Project)
        .ToListAsync();

            if (id == null)
            {
                return NotFound();
            }
                
            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }
            
            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            var UserEmail = HttpContext.Session.GetString("UserEmail");
            if (UserEmail == null)
            {
                return RedirectToAction("Login", "Account"); // Zaloguj użytkownika, jeśli nie ma sesji
            }
            var currentUser = _context.User.FirstOrDefault(u => u.Email == UserEmail);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home"); // Jeśli użytkownik nie istnieje, przekierowanie
            }

            var Role = HttpContext.Session.GetString("Role");

            var projects = _context.Project
                .Where(p => p.maker == currentUser.Email || Role == "Admin")
                .ToList() ?? new List<Project>();

            var users = _context.User.Where(u => u.Role == "User").ToList() ?? new List<User>();
            // Ustaw ViewBag z użytkownikami
            ViewBag.Users = users.Any()
        ? new SelectList(users, "Id", "Email")
        : new SelectList(Enumerable.Empty<SelectListItem>());

    // Ustaw ViewBag z projektami
    ViewBag.Projects = projects.Any()
        ? new SelectList(projects, "Id", "name")
        : new SelectList(Enumerable.Empty<SelectListItem>());

            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,title,Description,AssignedEmail,AssignedProject,status")] Models.Task task)
        {
            var UserEmail = HttpContext.Session.GetString("UserEmail");
            if (UserEmail == null)
            {
                return RedirectToAction("Login", "Account"); // Zaloguj użytkownika, jeśli nie ma sesji
            }
            var currentUser = _context.User.FirstOrDefault(u => u.Email == UserEmail);
            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home"); // Jeśli użytkownik nie istnieje, przekierowanie
            }

            var projects = _context.Project
                .Where(p => p.maker == currentUser.Email) // Przy założeniu, że masz ProjectManagerId w tabeli Project
                .ToList() ?? new List<Project>();

            var users = _context.User.Where(u => u.Role == "User").ToList() ?? new List<User>();
            // Ustaw ViewBag z użytkownikami
            ViewBag.Users = users.Any()
        ? new SelectList(users, "Id", "Email")
        : new SelectList(Enumerable.Empty<SelectListItem>());

            // Ustaw ViewBag z projektami
            ViewBag.Projects = projects.Any()
                ? new SelectList(projects, "Id", "name")
                : new SelectList(Enumerable.Empty<SelectListItem>());

            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            var users = _context.User.Where(u => u.Role == "User").ToList() ?? new List<User>();
            var projects = _context.Project.ToList() ?? new List<Project>();

            // Ustaw ViewBag z użytkownikami
            ViewBag.Users = users.Any()
                ? new SelectList(users, "Id", "Email")
                : new SelectList(Enumerable.Empty<SelectListItem>());

            // Ustaw ViewBag z projektami
            ViewBag.Projects = projects.Any()
                ? new SelectList(projects, "Id", "name")
                : new SelectList(Enumerable.Empty<SelectListItem>());
            return View(task);
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,title,Description,AssignedEmail,AssignedProject,status")] Models.Task task)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            if (task == null)
            {
                return NotFound();
            }
            var users = _context.User.Where(u => u.Role == "User").ToList() ?? new List<User>();
            var projects = _context.Project.ToList() ?? new List<Project>();

            // Ustaw ViewBag z użytkownikami
            ViewBag.Users = users.Any()
                ? new SelectList(users, "Id", "Email")
                : new SelectList(Enumerable.Empty<SelectListItem>());

            // Ustaw ViewBag z projektami
            ViewBag.Projects = projects.Any()
                ? new SelectList(projects, "Id", "name")
                : new SelectList(Enumerable.Empty<SelectListItem>());

            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task != null)
            {
                _context.Task.Remove(task);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
