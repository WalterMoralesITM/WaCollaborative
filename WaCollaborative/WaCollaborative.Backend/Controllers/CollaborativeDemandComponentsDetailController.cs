using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]")]
    public class CollaborativeDemandComponentsDetailController : GenericController<CollaborativeDemandComponentsDetail>//ControllerBase
    {
        private readonly DataContext _context;
        private readonly IGenericUnitOfWork<CollaborativeDemandComponentsDetail> _unitOfWork;

        public CollaborativeDemandComponentsDetailController(IGenericUnitOfWork<CollaborativeDemandComponentsDetail> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpPut("full")]
        public override async Task<IActionResult> PutAsync(CollaborativeDemandComponentsDetail collaboration)
        {
            var collaborationDetail = await _context.CollaborativeDemandComponentsDetail
                .FirstOrDefaultAsync(s => s.Id == collaboration.Id);
            if (collaborationDetail == null)
            {
                return NotFound();
            }
            collaborationDetail.Quantity = collaboration.Quantity;
            collaborationDetail.UpdateDate = DateTime.Now;
            _context.Update(collaborationDetail);
            await _context.SaveChangesAsync();
            return Ok(collaborationDetail);
        }

        [HttpGet("Portfolio")]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.CollaborativeDemandComponentsDetail
                .Include(d => d.User!)
                .Include(d => d.CollaborativeDemand!)
                    .ThenInclude(c => c.Product)
                 .Include(d => d.CollaborativeDemand!)
                    .ThenInclude(s => s.ShippingPoint)
                .AsQueryable();

            return Ok(await queryable
                .ToListAsync());
        }

        [HttpGet("Periods")]
        public async Task<IActionResult> GetPeriodsAsync([FromQuery] int collaborativeDemandId, [FromQuery] string userId)
        {
            var queryable = _context.CollaborativeDemandComponentsDetail
                .AsQueryable();
            queryable = queryable
        .Where(detail => detail.CollaborativeDemandId == collaborativeDemandId && detail.UserId == userId);

            return Ok(await queryable

                .ToListAsync());
        }
    }
}