using Microsoft.IdentityModel.Tokens;
using MyStoryTeamAPI.Db;
using MyStoryTeamAPI.Models.App;
using MyStoryTeamAPI.Models.Auth;
using MyStoryTeamAPI.Models.Db;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace MyStoryTeamAPI.Repository
{
    public class AuthRepository : RepositoryBase
    {
        private const string SALT = "SuperSecretPrefixIliSufix,doslovnonijebitno";

        private readonly JwtConfig jwtConfig;

        public AuthRepository(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor, JwtConfig jwtConfig) : base(dbContext, httpContextAccessor)
        {
            this.jwtConfig = jwtConfig;
        }

        public bool IsValidEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public bool IsEmailInUse(string email)
        {
            bool emailInUse = this.DbContext.Users.Where(u => u.Email == email).Any();
            return emailInUse;
        }

        public string GenerateSaltedPassword(string password)
        {
            return $"{SALT}&{password}";
        }

        public int RegisterUser(RegisterUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request.Email);
            ArgumentNullException.ThrowIfNull(request.Username);
            ArgumentNullException.ThrowIfNull(request.Password);
            ArgumentNullException.ThrowIfNull(request.RepeatPassword);

            if (this.IsValidEmailFormat(request.Email) == false)
            {
                throw new ArgumentException();
            }

            if (this.IsEmailInUse(request.Email) == true)
            {
                throw new ArgumentException();
            }

            if (request.Password != request.RepeatPassword)
            {
                throw new ArgumentException();
            }

            string password = this.GenerateSaltedPassword(request.Password);

            string hashString = BCrypt.Net.BCrypt.HashPassword(password);
            byte[] hashBytes = Encoding.UTF8.GetBytes(hashString);

            DbUser user = new DbUser
            {
                Username = request.Username,
                Email = request.Email,
                Password_Hash = hashBytes, //VARBINARY
                Created_At = DateTime.Now,
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();

            return user.ID_User;
        }

        public DbUser LoginUser(LoginUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request.Email);
            ArgumentNullException.ThrowIfNull(request.Password);

            if (this.IsValidEmailFormat(request.Email) == false)
            {
                throw new ArgumentException();
            }

            if (this.IsEmailInUse(request.Email) == false)
            {
                throw new ArgumentException();
            }

            DbUser userDetails = this.DbContext.Users.Where(u => u.Email == request.Email).Single();

            string password = this.GenerateSaltedPassword(request.Password);

            byte[] hashByte = userDetails.Password_Hash ?? throw new ArgumentException();
            string hashString = Encoding.UTF8.GetString(hashByte);

            bool valid = BCrypt.Net.BCrypt.Verify(password, hashString);

            if (valid == false)
            {
                throw new ArgumentException();
            }

            return userDetails;
        }

        public string GenerateJwtToken(DbUser user)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.Username ?? ""),
                new Claim("email_yes", user.Email ?? ""),
                new Claim("userid", user.ID_User.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(this.jwtConfig.Key!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: this.jwtConfig.Issuer,
                audience: this.jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
