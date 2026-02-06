using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("USERS")]
    public class DbUser
    {
        [Key]
        public int ID_User { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public byte[]? Password_Hash { get; set; }
        public DateTime? Created_At { get; set; }
    }
}
