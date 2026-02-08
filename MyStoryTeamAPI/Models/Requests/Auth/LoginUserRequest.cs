using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests.Auth
{
    public class LoginUserRequest
    {
        [Required]
        [MaxLength(500)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Password { get; set; }
    }
}
