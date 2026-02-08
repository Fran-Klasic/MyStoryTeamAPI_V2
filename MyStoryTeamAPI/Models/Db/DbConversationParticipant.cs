using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("Conversation_Participant")]
    public class DbConversationParticipant
    {
        [Key]
        public int ID_Conversation_Participant { get; set; }

        [ForeignKey(nameof(Conversation))]
        public int ID_Conversation { get; set; }

        [ForeignKey(nameof(User))]
        public int ID_User { get; set; }

        public DateTime Joined_At { get; set; }

        // Navigation
        public DbConversation? Conversation { get; set; }
        public DbUser? User { get; set; }
    }
}
