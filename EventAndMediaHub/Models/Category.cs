using System.ComponentModel.DataAnnotations;

namespace EventAndMediaHub.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
