using System.ComponentModel.DataAnnotations;

namespace ccc_projekt.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Start date")]
        public DateTime Start { get; set; }
        [Display(Name = "End date")]
        public DateTime? End { get; set; }
        [Display(Name = "Status")]
        public string status { get; set; }

        public string maker { get; set; }

    }
}
