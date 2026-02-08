namespace MyStoryTeamAPI.Models.Canvas
{
    public class CanvasDocument
    {
        public string Version { get; set; } = "1";
        public DateTime ExportedAt { get; set; }
        public List<CanvasElement> Elements { get; set; } = new();
    }

}
