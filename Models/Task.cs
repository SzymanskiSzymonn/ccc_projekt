using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ccc_projekt.Models
{
    public class Task
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string title { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [ForeignKey("User")]
        [Display(Name = "Assigned User Email")]
        public int? AssignedEmail { get; set; } // Klucz obcy
        public User? User { get; set; }
        [ForeignKey("Project")]
        [Display(Name = "Assigned Project")]
        public int AssignedProject { get; set; } // Klucz obcy
        public Project? Project { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }
    }
}
