using System.ComponentModel.DataAnnotations;

namespace SchoolHub.Models.ViewModels.Product
{
    public class RegisterProductViewModel
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string Name { get; set; } = null!;

        [StringLength(256)]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El precio no puede ser negativo")]
        [Display(Name = "Precio en puntos")]
        public int PricePoints { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        [Display(Name = "Stock")]
        public int Stock { get; set; }
    }
}
