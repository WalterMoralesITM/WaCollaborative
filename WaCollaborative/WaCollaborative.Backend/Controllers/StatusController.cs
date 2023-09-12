#region Using

using Microsoft.AspNetCore.Mvc;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.Backend.Controllers
{
    /// <summary>
    /// The Controller StatusController
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : GenericController<Status>
    {
        #region Constructor

        public StatusController(IGenericUnitOfWork<Status> unitOfWork) : base(unitOfWork)
        {
        }

        #endregion Constructor
    }
}