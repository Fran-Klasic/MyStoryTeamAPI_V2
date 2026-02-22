using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class CreateAIConversationRequest
    {
        [Required]
        public int ID_User { get; set; }
        public string? Title { get; set; }
    }
}
