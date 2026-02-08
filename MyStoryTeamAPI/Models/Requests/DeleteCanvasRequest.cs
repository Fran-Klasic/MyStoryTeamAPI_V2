using System.ComponentModel.DataAnnotations;

namespace MyStoryTeamAPI.Models.Requests
{
    public class DeleteCanvasRequest
    {
        [Required]
        public int? ID_Canvas { get; set; }
    }
}
