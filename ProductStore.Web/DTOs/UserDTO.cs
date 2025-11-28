using Microsoft.AspNetCore.Mvc.Rendering;
using ProductStore.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.DTOs
{
    public class UserDTO
    {
        public Guid? Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "Elcampo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(32, ErrorMessage = "Elcampo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [MaxLength(32, ErrorMessage = "Elcampo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Teléfono")]
        [MaxLength(32, ErrorMessage = "Elcampo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string PhoneNumber { get; set; } = null!;

        [MaxLength(64, ErrorMessage = "Elcampo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Rol")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string ProductStoreRoleId { get; set; }

        public ProductStoreRole? ProductStoreRole { get; set; }

        public IEnumerable<SelectListItem>? ProductStoreRoles { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
