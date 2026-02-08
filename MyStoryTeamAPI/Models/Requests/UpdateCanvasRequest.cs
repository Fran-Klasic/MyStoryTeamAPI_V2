using MyStoryTeamAPI.Models.Canvas;
using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class UpdateCanvasRequest
    {
        [Required]
        public int? ID_Canvas { get; set; }

        [MaxLength(50)]
        public string? Canvas_Name { get; set; }
        public CanvasDocument? Canvas_Data { get; set; }
        public DateTime Updated_At { get; set; } = DateTime.Now;
        public bool? Visibility { get; set; }
        public bool? Favorite { get; set; }
        public string? Background_Image { get; set; }
        public string? Background_Color { get; set; }
    }
}
