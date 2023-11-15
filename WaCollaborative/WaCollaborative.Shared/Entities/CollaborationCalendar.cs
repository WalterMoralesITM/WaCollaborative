using System.ComponentModel.DataAnnotations;

namespace WaCollaborative.Shared.Entities
{
    public class CollaborationCalendar
    {
        public int Id { get; set; }

        [Display(Name = "Fecha Inicial")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha Final")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime EndDate { get; set; }

        public InternalRole? InternalRole { get; set; }

        [Display(Name = "Rol")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una {0}.")]
        public int? InternalRoleId { get; set; }
    }
}
