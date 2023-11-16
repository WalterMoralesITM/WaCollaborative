using System.ComponentModel.DataAnnotations;

namespace WaCollaborative.Shared.Entities
{
    public class PortfolioCustomer
    {
        public int Id { get; set; }
        public Customer? Customer { get; set; }

        [Display(Name = "Cliente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? CustomerId { get; set; }

        public Portfolio? Portfolio { get; set; }

        [Display(Name = "Portafolio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? PortfolioId { get; set; }

        public ICollection<PortfolioCustomerProduct>? PortfolioCustomerProducts { get; set; }

        [Display(Name = "Productos")]
        public int ProductsNumber => PortfolioCustomerProducts == null ? 0 : PortfolioCustomerProducts.Count;
    }
}