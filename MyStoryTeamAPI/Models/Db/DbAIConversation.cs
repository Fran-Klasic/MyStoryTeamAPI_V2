using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("AI_Conversation")]
    public class DbAIConversation
    {
        [Key]
        public int ID_AI_Conversation { get; set; }

        [ForeignKey(nameof(User))]
        public int ID_User { get; set; }

        public DateTime Created_At { get; set; }

        // Navigation
        public DbUser? User { get; set; }
        public ICollection<DbAIMessage>? Messages { get; set; }
    }
}
