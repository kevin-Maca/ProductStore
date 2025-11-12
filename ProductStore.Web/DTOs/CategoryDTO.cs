using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = null!;


        [Display(Name = "Descripción")]
        public string? Description { get; set; }
    }
}
