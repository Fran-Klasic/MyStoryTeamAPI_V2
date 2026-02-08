using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("Message")]
    public class DbMessage
    {
        [Key]
        public int ID_Message { get; set; }

        [ForeignKey(nameof(Conversation))]
        public int ID_Conversation { get; set; }

        [ForeignKey(nameof(Sender))]
        public int ID_User_Sender { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime Created_At { get; set; }

        // Navigation
        public DbConversation? Conversation { get; set; }
        public DbUser? Sender { get; set; }
    }
}
