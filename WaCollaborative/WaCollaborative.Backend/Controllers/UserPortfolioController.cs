using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Helpers;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]")]
    public class UserPortfolioController : GenericController<User>
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly IGenericUnitOfWork<User> _unitOfWork;
        public UserPortfolioController(IGenericUnitOfWork<User> unitOfWork, DataContext context, IUserHelper userHelper, IMailHelper mailHelper) : base(unitOfWork, context)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("PortfolioAll")]
        public async Task<ActionResult> GetPortfolioAsync([FromQuery] PaginationDTO pagination)
        {
            try
            {
                var queryable = _context.Users
                                .Include(c => c.InternalRole)
                                .Include(c => c.CollaborativeDemandComponentsDetail!)
                                //    .ThenInclude(d => d.CollaborativeDemand)
                                //        .ThenInclude(d => d.ProductId)
                                //.Include(c => c.CollaborativeDemandComponentsDetail!)
                                //    .ThenInclude(p => p.CollaborativeDemand)
                                //        .ThenInclude(p => p.ShippingPoint)

                                .AsQueryable();

                //if (!string.IsNullOrWhiteSpace(pagination.Filter))
                //{
                //    queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
                //}

                return Ok(await queryable
                    //.OrderBy(c => c.Name)
                    .Paginate(pagination)
                    .ToListAsync());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}
