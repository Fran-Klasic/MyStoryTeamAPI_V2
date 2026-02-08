using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("Visited_Canvas")]
    public class DbVisitedCanvas
    {
        [Key]
        public int ID_Visited_Canvas { get; set; }

        [ForeignKey(nameof(User))]
        public int ID_User { get; set; }

        [ForeignKey(nameof(Canvas))]
        public int ID_Canvas { get; set; }

        public DateTime Visited_At { get; set; }

        // Navigation
        public DbUser? User { get; set; }
        public DbCanvas? Canvas { get; set; }
    }
}
