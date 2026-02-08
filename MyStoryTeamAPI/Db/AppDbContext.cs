using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyStoryTeamAPI.Models.Db;

namespace MyStoryTeamAPI.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Tables
        public DbSet<DbCanvas> Canvases { get; set; }
        public DbSet<DbAIConversation> AIConversations { get; set; }
        public DbSet<DbAIMessage> AIMessages { get; set; }
        public DbSet<DbConversation> Conversations { get; set; }
        public DbSet<DbConversationParticipant> ConversationParticipants { get; set; }
        public DbSet<DbMessage> Messages { get; set; }
        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbVisitedCanvas> VisitedCanvases { get; set; }

    }
}
                                                                   