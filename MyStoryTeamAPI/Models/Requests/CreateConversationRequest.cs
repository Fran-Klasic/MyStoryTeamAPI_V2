using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class CreateConversationRequest
    {
        [Required]
        public List<int>? ID_Users { get; set; }

    }
}
