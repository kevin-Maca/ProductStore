using ProductStore.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace ProductStore.Web.Data.Entities
{
    public class Category : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        public required string Name { get; set; }

        [MaxLength(64)]
        public string? Description { get; set; }

        public List<Product>? Product { get; set; }
    }
}