using MyStoryTeamAPI.Models.Canvas;
using MyStoryTeamAPI.Models.Db;
using Newtonsoft.Json;

namespace MyStoryTeamAPI.Models.Responses
{
    public class CanvasDetailsResponse
    {
        public int ID_Canvas { get; }
        public int ID_User { get; }
        public string Username { get; set; }
        public string Canvas_Name { get; }
        public string Canvas_Data { get; }
        public DateTime Created_At { get; }
        public DateTime Updated_At { get; }
        public bool Visibility { get; }
        public bool Favorite { get; }
        public string? Background_Image { get; }
        public string? Background_Color { get; }

        public CanvasDocument CanvasDataDetails { get; }

        public bool IsCanvasDataValid { get; }

        public CanvasDetailsResponse(DbCanvas dbCanvas)
        {
            ID_Canvas = dbCanvas.ID_Canvas;
            ID_User = dbCanvas.ID_User;
            Canvas_Name = dbCanvas.Canvas_Name!;
            Canvas_Data = dbCanvas.Canvas_Data!;
            Created_At = dbCanvas.Created_At;
            Updated_At = dbCanvas.Updated_At;
            Visibility = dbCanvas.Visibility;
            Favorite = dbCanvas.Favorite;
            Background_Image = dbCanvas.Background_Image;
            Background_Color = dbCanvas.Background_Color;
            Username = dbCanvas.User?.Username ?? "User";

            try
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new CanvasElementConverter());

                CanvasDataDetails = JsonConvert.DeserializeObject<CanvasDocument>(dbCanvas.Canvas_Data!, settings)
                    ?? throw new JsonException("Canvas data is null");

                IsCanvasDataValid = true;
            }
            catch (JsonException ex)
            {
                IsCanvasDataValid = false;
                throw new JsonException("Canvas data is not in a valid format. " + ex);
            }
        }
    }
}
