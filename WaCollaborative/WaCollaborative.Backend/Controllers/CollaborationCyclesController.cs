using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Helpers;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class CollaborationCyclesController : GenericController<CollaborationCycle>
    {
        private readonly DataContext _context;
        private readonly IGenericUnitOfWork<CollaborationCycle> _unitOfWork;
        public CollaborationCyclesController(IGenericUnitOfWork<CollaborationCycle> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<ActionResult> GetComboAsync()
        {
            return Ok(await _context.CollaborationCycles
                .OrderBy(p => p.Period)
                .ToListAsync());
        }

        [HttpGet("all")]
        public  async Task<ActionResult> GetAsync()
        {
            var queryable = _context.CollaborationCycles
                .Include(c => c.Status)
                .AsQueryable();

            return Ok(await queryable
                .OrderBy(c => c.Id)
                .ToListAsync());
        }

    }
}
