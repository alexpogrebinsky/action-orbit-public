using System.ComponentModel.DataAnnotations;

namespace mmDailyPlanner.Server.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Password { get; set; }
    }
}