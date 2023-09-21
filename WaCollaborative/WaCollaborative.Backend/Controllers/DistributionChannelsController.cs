using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistributionChannelsController : GenericController<DistributionChannel>
    {
        private readonly DataContext _context;

        public DistributionChannelsController(IGenericUnitOfWork<DistributionChannel> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
        }

        #region Methods

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.DistributionChannels
                .AsQueryable();

            var result = await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            DistributionChannel? distributionChannel = await _context.DistributionChannels
                .FirstOrDefaultAsync(c => c.Id == id);

            if (distributionChannel == null)
            {
                return NotFound();
            }

            return Ok(distributionChannel);
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.DistributionChannels
                .AsQueryable();

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        #endregion Methods
    }
}