#region Using

using Microsoft.AspNetCore.Mvc;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.Backend.Controllers
{

    /// <summary>
    /// The Controller StatesController
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : GenericController<State>
    {

        #region Constructor

        public StatesController(IGenericUnitOfWork<State> unitOfWork) : base(unitOfWork)
        {
        }

        #endregion Constructor

    }
}