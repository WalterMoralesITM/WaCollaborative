#region Using

using System.ComponentModel.DataAnnotations;

#endregion Using

namespace WaCollaborative.Shared.DTOs
{

    /// <summary>
    /// The class EmailDTO
    /// </summary>

    public class EmailDTO
    {

        #region Attributes

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
        public string Email { get; set; } = null!;

        #endregion Attributes

    }
}