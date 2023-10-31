using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class CollaborativeDemandComponentsDetail
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public decimal Quantity { get; set; }
        public DateTime UpdateDate { get; set; }
        public int YearMonth { get; set; }
        public CollaborativeDemand? CollaborativeDemand { get; set; }
        public int CollaborativeDemandId { get; set; }
        public User? User { get; set; }
        public string? UserId { get; set; }
    }
}
