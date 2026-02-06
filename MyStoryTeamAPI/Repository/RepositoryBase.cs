using MyStoryTeamAPI.Db;
using MyStoryTeamAPI.Models.Db;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyStoryTeamAPI.Repository
{
    public abstract class RepositoryBase
    {
        public RepositoryBase(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.DbContext = dbContext;
            this.HttpCotnext = httpContextAccessor;
        }

        public IHttpContextAccessor HttpCotnext { get; }
        public AppDbContext DbContext { get; }

        public DbUser GetCurrentUser()
        {
            Claim[] claims = this.HttpCotnext.HttpContext?.User.Claims.ToArray() ?? [];

            string id = claims.Where(c => c.Type == "userid").First().Value;
            string username = claims.Where(c => c.Type == "username").First().Value;
            string email = claims.Where(c => c.Type == "email_yes").First().Value;

            DbUser user = new DbUser
            { 
                ID_User = int.Parse(id),
                Username = username,
                Email = email,
            };

            return user;
        }
    }
}
