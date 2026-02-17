using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStoryTeamAPI.Models.Db;
using MyStoryTeamAPI.Models.Requests.Auth;
using MyStoryTeamAPI.Repository;

namespace MyStoryTeamAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepository;

        public AuthController(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult<string> LoginUser(LoginUserRequest request)
        {
            try
            {
                DbUser user = this._authRepository.LoginUser(request);
                string accessToken = this._authRepository.GenerateJwtToken(user);
                return this.Ok(accessToken);
            }
            catch (Exception)
            {
                return this.Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult RegisterUser(RegisterUserRequest request)
        {
            this._authRepository.RegisterUser(request);

            var userData = new LoginUserRequest()
            {
                Email = request.Email,
                Password = request.Password
            };

            try
            {
                DbUser user = this._authRepository.LoginUser(userData);
                string accessToken = this._authRepository.GenerateJwtToken(user);
                return this.Ok(accessToken);
            }
            catch (Exception)
            {
                return this.Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("test")]
        public ActionResult TestUser()
        {
            return this.Ok();
        }

        [Authorize]
        [HttpGet("user")]
        public ActionResult<string> GetCurrentUser()
        {
            string username = _authRepository.GetUsername() ?? "Username";
            return this.Ok(username);
        }
    }
}
