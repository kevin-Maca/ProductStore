using Microsoft.AspNetCore.Mvc.Rendering;
using ProductStore.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductStore.Web.DTOs
{
    public class ProductDTO
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Nombre del producto")]
        [Required]
        public string Name { get; set; } = string.Empty;


        [Display(Name = "Precio")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 9999999.99, ErrorMessage = "El campo {0} debe estar entre {1} y {2}.")]
        public decimal Price { get; set; }

        [Display(Name = "Stock disponible")]
        public int Stock { get; set; }
        public Category? Category { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public Guid categoryId { get; set; }

        public List<SelectListItem>? Categories { get; set; }
    }

    
}
