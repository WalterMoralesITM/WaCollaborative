using Microsoft.AspNetCore.Mvc;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementUnitsController : GenericController<MeasurementUnit>
    {
        public MeasurementUnitsController(IGenericUnitOfWork<MeasurementUnit> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
