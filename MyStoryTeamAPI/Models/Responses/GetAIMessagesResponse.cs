using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Responses
{
    public class GetAIMessagesResponse
    {
        [Required]
        public int ID_AI_Message { get; set; }
        [Required]
        public int ID_AI_Conversation { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public string? Type { get; set; }
    }
}
