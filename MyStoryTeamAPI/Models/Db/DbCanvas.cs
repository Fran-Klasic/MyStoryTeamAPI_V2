using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStoryTeamAPI.Models.Db
{
    [Table("Canvas")]
    public class DbCanvas
    {
        [Key]
        public int ID_Canvas { get; set; }

        [ForeignKey(nameof(User))]
        public int ID_User { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Canvas_Name { get; set; }

        [Required]
        public string? Canvas_Data { get; set; }

        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public bool Visibility { get; set; }
        public bool Favorite { get; set; }

        public string? Background_Color { get; set; }
        public string? Background_Image { get; set; }

        // Navigation
        public DbUser? User { get; set; }
    }
}
