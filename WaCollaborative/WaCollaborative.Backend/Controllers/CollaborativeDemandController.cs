#region using

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Enums;
using WaCollaborative.Shared.Helpers;

#endregion using

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]")]
    public class CollaborativeDemandController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public CollaborativeDemandController(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            try
            {
                var page = 1;
                var pageSize = 10;
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
                if (user == null)
                {
                    return BadRequest("User not valid.");
                }

                var queryable = _context.CollaborativeDemand
                .Include(cd => cd.Product)
                .Include(cd => cd.ShippingPoint)
                    .ThenInclude(sp => sp!.City) 
                .Include(cd => cd.ShippingPoint)
                    .ThenInclude(sp => sp!.Customer)
                        .ThenInclude(c => c!.DistributionChannel)                    
                .Include(cd => cd.CollaborativeDemandComponentsDetails)                   
                .AsQueryable();

                var result = await queryable
                    .Select(cd => new
                    {
                        CollaborativeDemandId = cd.Id,
                        CustomerName = cd.ShippingPoint!.Customer!.Name,
                        CustomerCode = cd.ShippingPoint!.Customer!.Code,
                        DistributionChannel = cd.ShippingPoint!.Customer!.DistributionChannel!.Name,                                                
                        ShippingPointName = cd.ShippingPoint!.Name,
                        CityName = cd.ShippingPoint.City!.Name, 
                        ProductName = cd.Product!.Name,
                        ProductCode = cd.Product.Code,
                        
                        CollaborativeDemandComponentsDetails = cd.CollaborativeDemandComponentsDetails!
                            .Select(detail => new
                            {
                                Quantity = detail.Quantity,
                                YearMonth = detail.YearMonth,
                                UserEmail = detail.User != null ? detail.User.Email : null,
                                UserId = detail.User != null ? detail.User.Id : null,
                            })
                    .ToList()


                    })
                    .Skip((page - 1) * pageSize) 
                    .Take(pageSize) 
                    .ToListAsync();

                var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Collaborator.ToString());
                if (!isAdmin)
                {
                    result = result.Where(s => s.CollaborativeDemandComponentsDetails.Any(detail => detail.UserEmail == User.Identity!.Name.ToString())).ToList();
                }
                return Ok(result);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("totalPages")]
        public async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
            if (user == null)
            {
                return BadRequest("User not valid.");
            }
            try
            {
                var queryable = _context.CollaborativeDemand
                    .Include(cd => cd.CollaborativeDemandComponentsDetails)
                    .AsQueryable();

                var result = await queryable
                        .Select(cd => new
                        {
                            CollaborativeDemandId = cd.Id,
                            CollaborativeDemandComponentsDetails = cd.CollaborativeDemandComponentsDetails!
                                .Select(detail => new
                                {
                                    UserEmail = detail.User != null ? detail.User.Email : null,
                                    UserId = detail.User != null ? detail.User.Id : null,
                                })
                        .ToList()
                        }).ToListAsync();

                var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Planner.ToString());
                if (!isAdmin)
                {
                    result = result.Where(s => s.CollaborativeDemandComponentsDetails.Any(detail => detail.UserEmail == User.Identity!.Name.ToString())).ToList();
                }

                double count = await queryable.CountAsync();
                double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
                return Ok(totalPages);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}