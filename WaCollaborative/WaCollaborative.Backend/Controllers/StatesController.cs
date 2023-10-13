#region Using

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.Backend.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : GenericController<State>
    {

        #region Attributes

        private readonly DataContext _context;

        #endregion Attributes

        #region Constructor

        public StatesController(IGenericUnitOfWork<State> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Methods

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.States
                .Include(c => c.Cities)
                .Where(x => x.Country!.Id == pagination.Id)
                .AsQueryable();

            var result = await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.States
                .Where(x=>x.Country!.Id == pagination.Id)
                .AsQueryable();
            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
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

        [AllowAnonymous]
        [HttpGet("combo/{countryId:int}")]
        public async Task<ActionResult> GetCombo(int countryId)
        {
            return Ok(await _context.States
                .Where(s => s.CountryId == countryId)
                .OrderBy(s => s.Name)
                .ToListAsync());
        }


        #endregion Methods

    }
}