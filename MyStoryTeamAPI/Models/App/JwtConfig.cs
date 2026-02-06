namespace MyStoryTeamAPI.Models.App
{
    public class JwtConfig
    {
        public JwtConfig(IConfiguration configuration)
        {
            var section = configuration.GetSection("JwtConfig");

            this.Issuer = section.GetValue<string>("Issuer");
            this.Audience = section.GetValue<string>("Audience");
            this.Key = section.GetValue<string>("Key");
        }

        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Key { get; set; }
    }
}
