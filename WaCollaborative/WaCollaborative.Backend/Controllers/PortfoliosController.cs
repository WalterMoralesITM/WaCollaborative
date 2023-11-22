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

            //if (queryable != null)
            //{
            //    var portfolios = await queryable.ToListAsync();
            //    await AddToDemandPlanAsync(portfolios);
            //}


            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        //private async Task<List<Portfolio>> AddToDemandPlanAsync(List<Portfolio> portfolios)
        //{
        //    CollaborativeDemand collaborativeDemand = new CollaborativeDemand();

        //    foreach (var portfolio in portfolios)
        //    {
        //        foreach (var portfolioCustomer in portfolio.PortfolioCustomers!)
        //        {
        //            foreach (var customer in portfolioCustomer.Customer)
        //            {
        //                foreach (var shippingPoint in customer.ShippingPoints)
        //                {
        //                    collaborativeDemand = new CollaborativeDemand();
        //                    //{
        //                    //    DemandTypeId = 1,  // Puedes asignar el valor adecuado
        //                    //    ShippingPointId = shippingPoint.Id,
        //                    //    // Otras propiedades...
        //                    //};

        //                    var collaborativeDemandDetail = new CollaborativeDemandComponentsDetail
        //                    {
        //                        Quantity = 10,  // Puedes asignar el valor adecuado
        //                        UpdateDate = DateTime.Now,
        //                        CollaborativeDemand = collaborativeDemand,
        //                        // Otras propiedades...
        //                    };

        //                    _context.CollaborativeDemands.Add(collaborativeDemand);
        //                    _context.CollaborativeDemandComponentsDetails.Add(collaborativeDemandDetail);
        //                }
        //            }
        //        }
        //    }
        //    await _context.SaveChangesAsync();
        //}

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
