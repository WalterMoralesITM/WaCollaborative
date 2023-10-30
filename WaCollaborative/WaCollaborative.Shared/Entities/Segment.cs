#region Using

using System.ComponentModel.DataAnnotations;

#endregion Using

namespace WaCollaborative.Shared.Entities
{

    /// <summary>
    /// The class entity Segment
    /// </summary>

    public class Segment
    {

        #region Attributes

        public int Id { get; set; }

        [Display(Name = "Segmento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public ICollection<Product>? Products { get; set; }

        #endregion Attributes

    }
}