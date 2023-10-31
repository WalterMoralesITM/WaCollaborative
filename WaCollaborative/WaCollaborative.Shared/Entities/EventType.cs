#region Using

using System.ComponentModel.DataAnnotations;

#endregion Using

namespace WaCollaborative.Shared.Entities
{

    /// <summary>
    /// The class entity EventType
    /// </summary>

    public class EventType
    {

        #region Attributes

        public int Id { get; set; }

        [Display(Name = "Tipo de Evento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public ICollection<CollaborativeDemand>? CollaborativeDemand { get; set; }
        #endregion Attributes

    }
}