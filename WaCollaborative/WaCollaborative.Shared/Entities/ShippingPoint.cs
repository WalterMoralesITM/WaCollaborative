using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class ShippingPoint
    {
        public int Id { get; set; }

        [Display(Name = "PuntoEnvío")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
        public Customer? Customer { get; set; }
        public int CustomerId { get; set; }
        public City? City { get; set; } 
        public int CityId { get; set; }
        public Status? Status { get; set; } 
        public int StatusId { get; set; }
    }
}
