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
    public class PortfolioCustomersController : GenericController<PortfolioCustomer>
    {
        private readonly IGenericUnitOfWork<PortfolioCustomer> _unitOfWork;
        private readonly DataContext _context;

        public PortfolioCustomersController(IGenericUnitOfWork<PortfolioCustomer> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.PortfolioCustomers
                .Include(p => p.Customer)                                                                                  
                .Where(x => x.Portfolio!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Customer!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .Include(p => p.Customer)
                .OrderBy(x => x.Customer!.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.PortfolioCustomers
                .Where(x => x.Portfolio!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Customer!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var portfolioCustomer = await _context.PortfolioCustomers
                .Include(p => p.Customer)
                .Include(s => s.Portfolio)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (portfolioCustomer == null)
            {
                return NotFound();
            }

            return Ok(portfolioCustomer);
        }
    }
}