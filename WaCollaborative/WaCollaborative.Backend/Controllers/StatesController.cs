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
    /// The Controller StatesController
    /// </summary>

    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : GenericController<State>
    {
        #region Attributes

        private readonly DataContext _context;

        #endregion Attributes

        #region Constructor

        public StatesController(IGenericUnitOfWork<State> unitOfWork, DataContext context) : base(unitOfWork)
        {
            _context = context;
        }

        #endregion Constructor

        #region Methods

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.States.Include(e => e.Cities).ToListAsync());
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var state = await _context.States
                 .Include(x => x.Cities)
                 .FirstOrDefaultAsync(x => x.Id == id);
            if (state is null)
            {
                return NotFound();
            }

            return Ok(state);
        }

        #endregion Methods
    }
}