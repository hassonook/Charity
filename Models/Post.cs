using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Charity.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Photos { get; set; }
        public string? Contact { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        [ForeignKey("CreatedBy")]
        public ApplicationUser Creator { get; set; }
        public string CreatedBy { get; set; }

    }
}
