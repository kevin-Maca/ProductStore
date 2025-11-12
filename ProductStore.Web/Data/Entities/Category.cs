using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }
    }
}