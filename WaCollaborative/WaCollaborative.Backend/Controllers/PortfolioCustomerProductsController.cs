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
    public class PortfolioCustomerProductsController : GenericController<PortfolioCustomerProduct>
    {
        private readonly DataContext _context;

        public PortfolioCustomerProductsController(IGenericUnitOfWork<PortfolioCustomerProduct> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("combo/{portfolioCustomerId:int}")]
        public async Task<ActionResult> GetComboAsync(int portfolioCustomerId)
        {
            return Ok(await _context.PortfolioCustomerProducts
                .Include(p => p.Product)
                .Where(c => c.PortfolioCustomerId == portfolioCustomerId)
                .OrderBy(c => c.Product!.Name)
                .ToListAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.PortfolioCustomerProducts
                .Include(p => p.Product)
                .Where(x => x.PortfolioCustomer!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .Include(p => p.Product)
                .OrderBy(x => x.Product!.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.PortfolioCustomerProducts
                .Include(p => p.Product)
                .Where(x => x.PortfolioCustomer!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product! .Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            PortfolioCustomerProduct? portfolioCustomerProduct = await _context.PortfolioCustomerProducts
                .Include(p => p.Product)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (portfolioCustomerProduct == null)
            {
                return NotFound();
            }

            return Ok(portfolioCustomerProduct);
        }
    }
}
