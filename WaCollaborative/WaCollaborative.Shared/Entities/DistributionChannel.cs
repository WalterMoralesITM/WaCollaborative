#region Using

using System.ComponentModel.DataAnnotations;

#endregion Using

namespace WaCollaborative.Shared.Entities
{

    /// <summary>
    /// The class entity DistributionChannel
    /// </summary>

    public class DistributionChannel
    {

        #region Attributes

        public int Id { get; set; }

        [Display(Name = "Canal de Distribución")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        #endregion Attributes

    }
}