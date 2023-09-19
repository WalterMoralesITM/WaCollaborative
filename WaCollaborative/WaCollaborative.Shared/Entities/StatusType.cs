#region Using

using System.ComponentModel.DataAnnotations;

#endregion Using

namespace WaCollaborative.Shared.Entities
{
    /// <summary>
    /// The class entity StatusType
    /// </summary>

    public class StatusType
    {

        #region Attributes

        public int Id { get; set; }

        [Display(Name = "Tipo Estado")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public ICollection<Status>? Status { get; set; }

        [Display(Name = "Estados")]
        public int StatusNumber => Status == null ? 0 : Status.Count;

        #endregion Attributes

    }
}