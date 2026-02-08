using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("Conversation")]
    public class DbConversation
    {
        [Key]
        public int ID_Conversation { get; set; }

        public DateTime Created_At { get; set; }

        // Navigation
        public ICollection<DbConversationParticipant>? Participants { get; set; }
        public ICollection<DbMessage>? Messages { get; set; }
    }
}
