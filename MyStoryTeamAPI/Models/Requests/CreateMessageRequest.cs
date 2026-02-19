using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class CreateMessageRequest
    {
        [Required]
        public int? ID_Sender { get; set; }

        [Required]
        public int? ID_Reciever { get; set; }

        [Required]
        [MaxLength(1000)]
        public string? Message { get; set; }
    }
}
