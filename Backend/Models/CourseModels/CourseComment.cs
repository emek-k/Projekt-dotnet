using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CourseHouse.Models;

namespace CourseHouse.Models
{
    [Table("CourseComments")]
    public class CourseComment
    {
        [Key]
        public int CourseCommentId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [StringLength(450, MinimumLength = 1, ErrorMessage = "Comment must be between 1 and 450 characters.")]
        public string CommentContent { get; set; } = string.Empty;
    }
}
