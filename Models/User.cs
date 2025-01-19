using System.ComponentModel.DataAnnotations;

namespace ccc_projekt.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(300, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 30 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
