using System.ComponentModel.DataAnnotations;

namespace WaCollaborative.Shared.Entities
{
    public class InternalRole
    {
        public int Id { get; set; }

        [Display(Name = "Nombre de Rol")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;

        public ICollection<CollaborationCalendar>? CollaborationCalendars { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
