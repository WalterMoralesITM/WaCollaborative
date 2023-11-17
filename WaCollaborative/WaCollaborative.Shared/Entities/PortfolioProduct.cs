using System.ComponentModel.DataAnnotations;

namespace WaCollaborative.Shared.Entities
{
    public class PortfolioProduct
    {
        public int Id { get; set; }
        public Product? Product { get; set; }

        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int ProductId { get; set; }

        public Portfolio? Portfolio { get; set; }

        [Display(Name = "Portafolio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? PortfolioId { get; set; }
    }
}