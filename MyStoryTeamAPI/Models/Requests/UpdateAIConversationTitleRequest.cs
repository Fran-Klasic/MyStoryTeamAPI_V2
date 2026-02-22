using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class UpdateAIConversationTitleRequest
    {
        [Required]
        public int ID_AI_Conversation { get; set; }

        [Required]
        public string? Title { get; set; }
    }
}
