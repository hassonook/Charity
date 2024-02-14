using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Charity.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime CommentedOn { get; set; } = DateTime.Now;
        [ForeignKey("CommentedBy")]
        public ApplicationUser Commentor { get; set; }
        public string CommentedBy { get; set; }

    }
}
