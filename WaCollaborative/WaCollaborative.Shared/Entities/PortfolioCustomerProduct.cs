using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class PortfolioCustomerProduct
    {
        public int Id { get; set; }
        public PortfolioCustomer? PortfolioCustomer { get; set; }

        [Display(Name = "Cliente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? PortfolioCustomerId { get; set; }

        public Product? Product { get; set; }

        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? ProductId { get; set; }
    }
}
