#region Using

using Microsoft.AspNetCore.Mvc;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.Backend.Controllers
{

    /// <summary>
    /// The Controller CitiesController
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : GenericController<City>
    {

        #region Constructor

        public CitiesController(IGenericUnitOfWork<City> unitOfWork) : base(unitOfWork)
        {
        }

        #endregion Constructor

    }
}