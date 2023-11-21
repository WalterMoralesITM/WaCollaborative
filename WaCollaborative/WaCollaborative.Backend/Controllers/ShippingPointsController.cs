using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class ShippingPointsController : GenericController<ShippingPoint>
    {
        private readonly DataContext _context;
        private readonly IGenericUnitOfWork<ShippingPoint> _unitOfWork;

        public ShippingPointsController(IGenericUnitOfWork<ShippingPoint> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
       
        [HttpGet("combo/{customerId:int}")]
        public async Task<ActionResult> GetComboAsync(int customerId)
        {
            return Ok(await _context.ShippingPoints
                .Include(s => s.City)
                .Where(s => s.CustomerId == customerId)
                .OrderBy(s => s.Name)
                .ToListAsync());
        }

        //[HttpGet("{id}")]
        //public override async Task<IActionResult> GetAsync(int id)
        //{
        //    var portfolio = await _unitOfWork.GetPortfolioAsync(id);
        //    if (portfolio == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(portfolio);
        //}

    }
}
