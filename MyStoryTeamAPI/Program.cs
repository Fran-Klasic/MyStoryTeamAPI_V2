using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyStoryTeamAPI.Db;
using MyStoryTeamAPI.Models.App;
using MyStoryTeamAPI.Models.Canvas;
using MyStoryTeamAPI.Repository;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

JwtConfig jwtConfig = new JwtConfig(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer((options) =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer!,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key!))
        };
    });

builder.Services.AddAuthorization();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(config =>
    {
        config.AddDefaultPolicy(policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod();
        });
    });
}
else
{
    builder.Services.AddCors(config =>
    {
        config.AddDefaultPolicy(policy =>
        {
            policy
                .WithOrigins("https://fran-klasic.github.io")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        Environment.GetEnvironmentVariable("DATABASE_CONNECTION") ?? builder.Configuration.GetConnectionString("Database")
    )
);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling =
            ReferenceLoopHandling.Ignore;
        options.SerializerSettings.Converters.Add(new CanvasElementConverter());
    });


#region REPOSITORIES

builder.Services.AddSingleton(jwtConfig);
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new OpenAI.OpenAIClient(Environment.GetEnvironmentVariable("OPENAI_KEY") ?? config["OpenAI:ApiKey"]);
});

builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<CanvasRepository>();
builder.Services.AddScoped<ConversationsRepository>();
builder.Services.AddScoped<AiConversationsRepository>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();