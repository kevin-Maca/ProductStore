using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.DTOs
{
    public class ProductStoreRoleDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; }

        public List<PermissionsForRoleDTO>? Permissions { get; set; }

        public string? PermissionIds { get; set; }

        public List<CategoriesForRoleDTO>? Categories { get; set; }

        public string? CategoryIds { get; set; }
    }

    public class PermissionsForRoleDTO : PermissionDTO
    {
        public bool Selected { get; set; }
    }
    public class CategoriesForRoleDTO : CategoryDTO
    {
        public bool Selected { get; set; }
    }
}
