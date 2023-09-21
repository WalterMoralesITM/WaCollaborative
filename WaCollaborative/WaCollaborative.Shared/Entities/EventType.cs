using System.ComponentModel.DataAnnotations;

namespace WaCollaborative.Shared.Entities
{
    public class EventType
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Evento")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; } = null!;
    }
}