using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.Data.Entities
{
    public class ProductStoreRole
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        [Required]
        public required string Name { get; set; }

        public ICollection<RolePermission>? RolePermission { get; set; }
    }
}
