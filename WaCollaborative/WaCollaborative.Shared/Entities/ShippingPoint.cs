#region Using

using System.ComponentModel.DataAnnotations;

#endregion Using

namespace WaCollaborative.Shared.Entities
{

    /// <summary>
    /// The class entity ShippingPoint
    /// </summary>

    public class ShippingPoint
    {

        #region Attributes

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

        #endregion Attributes

    }
}