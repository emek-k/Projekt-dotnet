using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseHouse.Models;

namespace CourseHouse.Models
{
    [Table("Pictures")]
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }

        [Required]
        [ForeignKey("Author")]
        public User User { get; set; }

        [Required]
        [Url(ErrorMessage = "Incorrect URL format.")]
        [StringLength(255, ErrorMessage = "URL length must not exceed 255 characters.")]
        public string ContentUrl { get; set; } = string.Empty;
    }
}
