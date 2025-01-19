
using Microsoft.EntityFrameworkCore;
using ccc_projekt.Models;

namespace ccc_projekt.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ccc_projekt.Models.Project> Project { get; set; } = default!;
        public DbSet<ccc_projekt.Models.Task> Task { get; set; } = default!;
        public DbSet<ccc_projekt.Models.User> User { get; set; } = default!;

    
       
    }
}
