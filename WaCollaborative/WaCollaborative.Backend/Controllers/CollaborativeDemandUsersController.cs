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
    [Route("/api/[controller]")]
    public class CollaborativeDemandUsersController : GenericController<CollaborativeDemandUsers>
    {
        private readonly DataContext _context;
        private readonly IGenericUnitOfWork<CollaborativeDemandUsers> _unitOfWork;
        public CollaborativeDemandUsersController(IGenericUnitOfWork<CollaborativeDemandUsers> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Products")]
        public async Task<ActionResult> GetProductsAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.CollaborativeDemand!
                                 .Include(c => c.Product)
                            .AsQueryable();

            return Ok(await queryable
                .OrderBy(u => u.Product!.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("ShippingPoints")]
        public async Task<ActionResult> GetShippingPointsAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.CollaborativeDemand!
                                 .Include(c => c.Product)
                            .AsQueryable();

            return Ok(await queryable
                .OrderBy(u => u.Product!.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet()]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {

            var queryable = _context.Users
                                .Include(u => u.InternalRole)
                                .Include(u => u.CollaborativeDemandUsers!)
                                 .ThenInclude(c => c.CollaborativeDemand)
                            .AsQueryable();

            return Ok(await queryable
                .OrderBy(u => u.Email)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("{id:int}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var queryable = _context.CollaborativeDemandUsers
                                .Include(c => c.User)
                                    .ThenInclude(u => u.InternalRole)
                                    .Where(c => c.CollaborativeDemandId == id)
                                .AsQueryable();

            return Ok(await queryable
                .ToListAsync());
        }

        [HttpGet("Detail")]
        public async Task<ActionResult> GetDetailAsync([FromQuery] PaginationDTO pagination, string userId)
        {
            var queryable = _context.CollaborativeDemandUsers!
                                .Include(u => u.CollaborativeDemand)
                                    .ThenInclude(p => p.Product)
                                .Include(u => u.CollaborativeDemand!)
                                .ThenInclude(s => s.ShippingPoint)
                                .Include(u => u.User)
                                .Where(u => u.UserId == userId)
                            .AsQueryable();

            return Ok(await queryable
                .OrderBy(u => u.CollaborativeDemandId)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.CollaborativeDemandUsers.AsQueryable();

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }
    }
}
