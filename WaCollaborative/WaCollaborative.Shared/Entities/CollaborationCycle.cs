using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class CollaborationCycle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Período")]
        public int Period { get; set; }
        public Status? Status { get; set; }
        public int StatusId { get; set; }
        public ICollection<CollaborationCalendar>? CollaborationCalendars { get; set; }
    }
}
