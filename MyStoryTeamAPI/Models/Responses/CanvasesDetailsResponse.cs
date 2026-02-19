using MyStoryTeamAPI.Models.Db;

namespace MyStoryTeamAPI.Models.Responses
{
    public class CanvasesDetailsResponse
    {
        public int ID_Canvas { get; }
        public int ID_User { get; }
        public string Username { get; set; }
        public string Canvas_Name { get; }
        public DateTime Created_At { get; }
        public DateTime Updated_At { get; }
        public bool Visibility { get; }
        public bool Favorite { get; }
        public string? Background_Image { get; }
        public string? Background_Color { get; }

        public CanvasesDetailsResponse(DbCanvas canvas)
        {
            ID_Canvas = canvas.ID_Canvas;
            ID_User = canvas.ID_User;
            Username = canvas.User?.Username ?? "User";
            Canvas_Name = canvas.Canvas_Name ?? "Untitled";
            Created_At = canvas.Created_At;
            Updated_At = canvas.Updated_At;
            Visibility = canvas.Visibility;
            Favorite = canvas.Favorite;
            Background_Image = canvas.Background_Image;
            Background_Color = canvas.Background_Color;
        }
    }
}
