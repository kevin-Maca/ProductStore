using ProductStore.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.Data.Entities
{
    public class ProductStoreRole : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        [Required]
        public required string Name { get; set; }

        public ICollection<RolePermission>? RolePermission { get; set; }
        public ICollection<RoleCategory>? RoleCategories { get; set; }
    }
}
