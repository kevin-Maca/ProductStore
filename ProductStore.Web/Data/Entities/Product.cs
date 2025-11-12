using ProductStore.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStore.Web.Data.Entities
{
    public class Product : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(64)]
        public required string Name { get; set; }

        [Column(TypeName = "DECIMAL(10,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }

        public required Guid categoryId { get; set; }

        public Category? Category { get; set; }
    }
}
