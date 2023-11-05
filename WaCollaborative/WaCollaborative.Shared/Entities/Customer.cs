#region Using

using System.ComponentModel.DataAnnotations;

#endregion Using

namespace WaCollaborative.Shared.Entities
{

    /// <summary>
    /// The class entity Customer
    /// </summary>

    public class Customer
    {

        #region Attributes

        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        [Display(Name = "Código")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        public string Code { get; set; } = null!;

        public DistributionChannel? DistributionChannel { get; set; }

        [Display(Name = "Canal de Distribución")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int DistributionChannelId { get; set; }

        public ICollection<ShippingPoint>? ShippingPoint { get; set; }
        
        #endregion

    }
}