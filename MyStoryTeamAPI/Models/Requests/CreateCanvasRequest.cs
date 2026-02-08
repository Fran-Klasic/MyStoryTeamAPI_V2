using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class CreateCanvasRequest
    {
        [MaxLength(50)]
        public string Canvas_Name { get; set; } = "Untitled";

        public string? Background_Image { get; set; }
        public string? Background_Color { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
    }
}
