using Microsoft.AspNetCore.Http.HttpResults;
using MyStoryTeamAPI.Db;
using MyStoryTeamAPI.Models.Canvas;
using MyStoryTeamAPI.Models.Db;
using MyStoryTeamAPI.Models.Responses;
using Newtonsoft.Json;

namespace MyStoryTeamAPI.Repository
{
    public class CanvasRepository : RepositoryBase
    {
        public CanvasRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
        }

        public int CreateCanvas(Models.Requests.CreateCanvasRequest createCanvasRequest)
        {
            DbCanvas canvas = new DbCanvas
            {
                ID_User = this.GetCurrentUser().ID_User,
                Canvas_Name = createCanvasRequest.Canvas_Name,
                Background_Image = createCanvasRequest.Background_Image ?? " ",
                Background_Color = createCanvasRequest.Background_Color ?? " ",
                Created_At = createCanvasRequest.Created_At,
                Updated_At = createCanvasRequest.Created_At,
                Canvas_Data = "",
                Visibility = false,
                Favorite = false,
            };

            this.DbContext.Canvases.Add(canvas);
            this.DbContext.SaveChanges();
            return canvas.ID_Canvas;
        }

        private DbCanvas? GetCanvasById(int? id)
        {
            DbCanvas? canvas = this.DbContext.Canvases.Where(c => c.ID_Canvas == id).FirstOrDefault();
            if (canvas == null)
            {
                return null;
            }
            return canvas;
        }

        public int? UpdateCanvas(Models.Requests.UpdateCanvasRequest updateCanvasRequest)
        {
            DbUser currentUser = this.GetCurrentUser();
            DbCanvas? canvas = this.GetCanvasById(updateCanvasRequest.ID_Canvas);
            if (canvas == null)
            {
                return null;
            }
            if (currentUser.ID_User != canvas.ID_User)
            {
                return null;
            }

            canvas.Canvas_Name = updateCanvasRequest.Canvas_Name ?? canvas.Canvas_Name;

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new CanvasElementConverter());

            canvas.Canvas_Data = JsonConvert.SerializeObject(updateCanvasRequest.Canvas_Data, settings);

            canvas.Updated_At = updateCanvasRequest.Updated_At;
            canvas.Visibility = updateCanvasRequest.Visibility ?? canvas.Visibility;
            canvas.Favorite = updateCanvasRequest.Favorite ?? canvas.Favorite;
            canvas.Background_Image = updateCanvasRequest.Background_Image ?? canvas.Background_Image;
            canvas.Background_Color = updateCanvasRequest.Background_Color ?? canvas.Background_Color;

            this.DbContext.Canvases.Update(canvas);
            this.DbContext.SaveChanges();
            return canvas.ID_Canvas;
        }

        public int? DeleteCanvas(int? id)
        {
            DbUser currentUser = this.GetCurrentUser();
            DbCanvas? canvas = this.GetCanvasById(id);
            if (canvas == null)
            {
                return null;
            }
            if (currentUser.ID_User != canvas.ID_User)
            {
                return null;
            }
            this.DbContext.Canvases.Remove(canvas);
            this.DbContext.SaveChanges();
            return canvas.ID_Canvas;
        }

        public CanvasDetailsResponse? GetCanvasDetails(int id)
        {
            DbCanvas? canvas = this.GetCanvasById(id);
            DbUser currentUser = this.GetCurrentUser();
            if (canvas == null)
            {
                return null;
            }
            if (currentUser.ID_User != canvas.ID_User && canvas.Visibility == false)
            {
                return null;
            }
            return new CanvasDetailsResponse(canvas)
            {
                Username = DbContext.Users
                    .Where(u => u.ID_User == canvas.ID_User)
                    .Select(u => u.Username)
                    .FirstOrDefault() ?? "User"
            };
        }

        public List<CanvasesDetailsResponse> GetAllCanvases()
        {
            //From user id get all canvases that belong to that user and return them as a list of CanvasesDetailsResponse
            DbUser currentUser = this.GetCurrentUser();
            var response = this.DbContext.Canvases
                .Where(c => c.ID_User == currentUser.ID_User)
                .Select(c => new CanvasesDetailsResponse(c)
                {
                    Username = currentUser.Username ?? "User"
                })
                .ToList();
            return response;
        }

        public List<CanvasesDetailsResponse> GetAllPublicCanvases()
        {
            return DbContext.Canvases
                .Where(c => c.Visibility == true)
                .Join(
                    DbContext.Users,
                    canvas => canvas.ID_User,
                    user => user.ID_User,
                    (canvas, user) => new CanvasesDetailsResponse(canvas)
                    {
                        Username = user.Username ?? "User"
                    })
                .ToList();
        }


    }
}
