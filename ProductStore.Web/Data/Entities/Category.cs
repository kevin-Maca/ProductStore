using ProductStore.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.Data.Entities
{
    public class Category : IId
    {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }
    }
}