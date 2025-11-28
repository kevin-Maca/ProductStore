using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.DTOs
{
    public class ChangePasswordDTO
    {
        [Display(Name = "Contraseña Actual")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener ina longitud mínima de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "Nueva Contraseña")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener ina longitud mínima de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Nueva Contraseña")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener ina longitud mínima de {1} carácteres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    }
}
