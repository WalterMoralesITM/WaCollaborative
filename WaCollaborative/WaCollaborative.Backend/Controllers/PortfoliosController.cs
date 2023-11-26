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
    public class PortfoliosController : GenericController<Portfolio>
    {
        private readonly IGenericUnitOfWork<Portfolio> _unitOfWork;
        private readonly DataContext _context;

        public PortfoliosController(IGenericUnitOfWork<Portfolio> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<ActionResult> GetComboAsync()
        {
            return Ok(await _context.Portfolios
                .OrderBy(c => c.Name)
                .ToListAsync());
        }

        /*versión 1*/
        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Portfolios
                .Include(c => c.PortfolioCustomers!)
                .ThenInclude(cu => cu.Customer!)
                .ThenInclude(s => s.ShippingPoint)
                .Include(c => c.PortfolioProducts)
                .Include(c => c.Users)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync());
        }
        
        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Portfolios.AsQueryable();

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
            var portfolio = await _unitOfWork.GetPortfolioAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return Ok(portfolio);
        }
    }
}
