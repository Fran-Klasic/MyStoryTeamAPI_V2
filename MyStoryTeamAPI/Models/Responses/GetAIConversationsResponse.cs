using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Responses
{
    public class GetAIConversationsResponse
    {
        [Required]
        public int ID_AI_Conversation { get; set; }
        [Required]
        public string? Title { get; set; }
    }
}
