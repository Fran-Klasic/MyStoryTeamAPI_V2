using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class AddNewUserRequest
    {
        [Required]
        public int? ID_User { get; set; }

        [Required]
        public int? ID_Conversation { get; set; }

        public DateTime Joined_At { get; set; } = DateTime.Now;
    }
}
