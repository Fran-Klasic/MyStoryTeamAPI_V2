using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Responses
{
    public class GetAllConversationsResponse
    {
        [Required]
        public int? ID_Conversation { get; set; }
        [Required]
        public int? ID_User { get; set; }
        public DateTime? Created_At { get; set; }
    }
}
