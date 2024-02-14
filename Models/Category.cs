using MessagePack;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Charity.Models
{
    public class Category
    {
        public int Id { get; set; }
        [DataType(DataType.Text)]
        public string? Name { get; set; }
        [DisplayName("Parent")]
        public int? ParentId { get; set; }
        public Category? Parent { get; set; }
    }
}
