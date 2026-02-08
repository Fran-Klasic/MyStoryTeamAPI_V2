using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("AI_Message")]
    public class DbAIMessage
    {
        [Key]
        public int ID_AI_Message { get; set; }

        [ForeignKey(nameof(AIConversation))]
        public int ID_AI_Conversation { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime Created_At { get; set; }

        // Navigation
        public DbAIConversation? AIConversation { get; set; }
    }
}
