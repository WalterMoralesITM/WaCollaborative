using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class CollaborativeDemandDemo
    {
        public int Id { get; set; }

        [Display(Name = "Cliente")]        
        public string CustomerName { get; set; } = null!;

        [Display(Name = "Producto")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "Ciudad")]
        public string CityName { get; set; } = null!;
        public int YearMonth { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Quantity { get; set; }

    }
}
