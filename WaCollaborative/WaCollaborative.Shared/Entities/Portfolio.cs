using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }

        [Display(Name = "Portafolio")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
        public ICollection<PortfolioCustomer>? PortfolioCustomers { get; set; }

        public ICollection<PortfolioProduct>? PortfolioProducts { get; set; }

        public ICollection<User>? Users { get; set; }

        [Display(Name = "Clientes")]
        public int CustomersNumber => PortfolioCustomers == null ? 0 : PortfolioCustomers.Count;

        [Display(Name = "Productos")]
        public int ProductsNumber => PortfolioProducts == null ? 0 : PortfolioProducts.Count;
        
    }
}
