using Microsoft.AspNetCore.SignalR.Protocol;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("Users")]
    public class DbUser
    {
        [Key]
        public int ID_User { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public byte[]? Password_Hash { get; set; }
        public DateTime? Created_At { get; set; }

        // Navigation
        public ICollection<DbConversationParticipant>? ConversationParticipants { get; set; }
        public ICollection<DbMessage>? SentMessages { get; set; }
        public ICollection<DbCanvas>? Canvases { get; set; }
        public ICollection<DbAIConversation>? AIConversations { get; set; }
    }
}