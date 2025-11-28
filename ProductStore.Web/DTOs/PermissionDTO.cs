using System.ComponentModel.DataAnnotations;

namespace ProductStore.Web.DTOs
{
    public class PermissionDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Permiso")]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string Description { get; set; } = null!;

        [Display(Name = "Módulo")]
        public string Module { get; set; } = null!;
    }
}
