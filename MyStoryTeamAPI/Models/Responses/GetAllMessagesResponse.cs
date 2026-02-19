using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Responses
{
    public class GetAllMessagesResponse
    {
        [Required]
        public int? ID_Message { get; set; }
        [Required]
        public int? ID_Conversation { get; set; }
        [Required]
        public int? ID_User_Sender { get; set; }
        [Required]
        public string? Message_Content { get; set; }
        [Required]
        public DateTime? Created_At { get; set; }
    }
}
