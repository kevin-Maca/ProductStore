using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        public string Name { get; set; } = null!;

        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} carácteres")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }
    }
}
