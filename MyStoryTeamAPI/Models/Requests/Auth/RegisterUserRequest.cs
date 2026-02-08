using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests.Auth
{
    public class RegisterUserRequest
    {
        [Required]
        [MaxLength(500)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Password { get; set; }

        [Required]
        [MaxLength(500)]
        public string? RepeatPassword { get; set; }
    }
}
