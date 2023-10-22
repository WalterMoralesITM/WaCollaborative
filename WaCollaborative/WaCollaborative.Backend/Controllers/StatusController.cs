#region Using

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Helpers;
using WaCollaborative.Backend.Interfaces;

#endregion Using

namespace WaCollaborative.Backend.Controllers
{
    /// <summary>
    /// The Controller StatusController
    /// </summary>

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class StatusController : GenericController<Status>
    {

        #region Attributes

        private readonly DataContext _context;

        #endregion Attributes

        #region Constructor

        public StatusController(IGenericUnitOfWork<Status> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Methods

        [AllowAnonymous]
        [HttpGet("combo/{statusTypeId:int}")]
        public async Task<ActionResult> GetComboAsync(int statusTypeId)
        {
            return Ok(await _context.Status
                .Where(s => s.StatusTypeId == statusTypeId)
                .OrderBy(s => s.Name)
                .ToListAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Status
               .Include(x => x.StatusType)
               .Where(x => x.StatusType!.Id == pagination.Id)
               .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(x => x.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Status
               .Where(x => x.StatusType!.Id == pagination.Id)
               .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var status = await _context.Status
                .Include(s => s.StatusType)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (status == null)
            {
                return NotFound();
            }
            return Ok(status);
        }

        #endregion Methods

    }
}