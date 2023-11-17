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
    public class PortfolioProductsController : GenericController<PortfolioProduct>
    {
        private readonly IGenericUnitOfWork<PortfolioProduct> _unitOfWork;
        private readonly DataContext _context;

        public PortfolioProductsController(IGenericUnitOfWork<PortfolioProduct> unitOfWork, DataContext context) 
            : base(unitOfWork, context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("combo/{portfolioId:int}")]
        public async Task<ActionResult> GetComboAsync(int portfolioId)
        {
            return Ok(await _context.PortfolioProducts
                .Include(p => p.Product)
                .Where(s => s.PortfolioId == portfolioId)
                .OrderBy(s => s.Product!.Name)
                .ToListAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.PortfolioProducts
                .Include(p => p.Product)
                .Where(x => x.Portfolio!.Id == pagination.Id)
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
            var queryable = _context.PortfolioProducts
                .Where(x => x.Portfolio!.Id == pagination.Id)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var state = await _context.PortfolioProducts
                .Include(p => p.Product)
                .Include(s => s.Portfolio)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(state);
        }
    }
}
