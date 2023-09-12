#region Using

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

#endregion Using


namespace WaCollaborative.Backend.Controllers
{

    /// <summary>
    /// The Controller StatusTypeController
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class StatusTypeController : GenericController<StatusType>
    {
        #region Attributes

        private readonly DataContext _context;

        #endregion Attributes

        #region Constructor

        public StatusTypeController(IGenericUnitOfWork<StatusType> unitOfWork, DataContext context) : base(unitOfWork)
        {
            _context = context;
        }

        #endregion Constructor

        #region Methods

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.StatusType
                .Include(c => c.Status)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var statustype = await _context.StatusType
                .Include(c => c.Status!)               
                .FirstOrDefaultAsync(c => c.Id == id);
            if (statustype == null)
            {
                return NotFound();
            }
            return Ok(statustype);
        }

        #endregion Methods

    }
}