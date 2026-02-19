using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class CreateConversationRequest
    {
        [Required]
        public List<int>? ID_Users { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
    }
}
