using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Controllers
{
    public class CollaborationCalendarsController : GenericController<CollaborationCalendar>
    {
        private readonly DataContext _context;

        public CollaborationCalendarsController(IGenericUnitOfWork<CollaborationCalendar> unitOfWork, DataContext context) 
            : base(unitOfWork, context)
        {
            _context = context;
        }
    }
}
