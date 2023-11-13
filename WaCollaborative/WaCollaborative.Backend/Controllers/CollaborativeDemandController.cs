#region using

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Enums;

#endregion using

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]")]
    public class CollaborativeDemandController : GenericController<CollaborativeDemand>
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public CollaborativeDemandController(IGenericUnitOfWork<CollaborativeDemand> unitOfWork, DataContext context, IUserHelper userHelper) : base(unitOfWork, context)
        {
            _context = context;
            _userHelper = userHelper;
        }
        

        [AllowAnonymous]
        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            try
            {
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

                var collaborativeDemands = await queryable.ToListAsync();

                var result = FlattenCollaborativeDemands(collaborativeDemands);

                var isAdmin = await _userHelper.IsUserInRoleAsync(user, UserType.Collaborator.ToString());
                if (!isAdmin)
                {
                    result = result.Where(s => s.UserEmail == User.Identity!.Name!.ToString()).ToList();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<CollaborativeDemandDTO> FlattenCollaborativeDemands(List<CollaborativeDemand> collaborativeDemands)
        {
            var result = new List<CollaborativeDemandDTO>();

            foreach (var collaborativeDemand in collaborativeDemands)
            {
                foreach (var detail in collaborativeDemand.CollaborativeDemandComponentsDetails!)
                {
                    var collaborativeDemandDTO = new CollaborativeDemandDTO
                    {
                        CollaborativeDemandId = collaborativeDemand.Id,
                        CustomerName = collaborativeDemand.ShippingPoint!.Customer!.Name,
                        CustomerCode = collaborativeDemand.ShippingPoint!.Customer!.Code,
                        DistributionChannel = collaborativeDemand.ShippingPoint!.Customer!.DistributionChannel!.Name,
                        ShippingPointName = collaborativeDemand.ShippingPoint!.Name,
                        CityName = collaborativeDemand.ShippingPoint.City!.Name,
                        ProductName = collaborativeDemand.Product!.Name,
                        ProductCode = collaborativeDemand.Product.Code,
                        UserEmail = "wmorales@yopmail.com",
                        UserId = "1",
                        CollaborativeDemandDetailId = detail.Id,
                        YearMonth = detail.YearMonth, 
                        Quantity = detail.Quantity 
                    };

                    result.Add(collaborativeDemandDTO);
                }
            }

            return result;
        }

        private List<CollaborativeDemandDTO> PivotCollaborativeDemands(List<CollaborativeDemand> collaborativeDemands)
        {
            var result = new List<CollaborativeDemandDTO>();

            // Crear un diccionario para almacenar los valores de YearMonth y Quantity
            var yearMonthQuantities = new Dictionary<int, decimal>();

            foreach (var collaborativeDemand in collaborativeDemands)
            {
                foreach (var detail in collaborativeDemand.CollaborativeDemandComponentsDetails)
                {
                    // Actualizar el diccionario con los valores de YearMonth y Quantity
                    if (yearMonthQuantities.ContainsKey(detail.YearMonth))
                    {
                        yearMonthQuantities[detail.YearMonth] += detail.Quantity;
                    }
                    else
                    {
                        yearMonthQuantities[detail.YearMonth] = detail.Quantity;
                    }
                }
            }

            foreach (var collaborativeDemand in collaborativeDemands)
            {
                var collaborativeDemandDTO = new CollaborativeDemandDTO
                {
                    CollaborativeDemandId = collaborativeDemand.Id,
                    CustomerName = collaborativeDemand.ShippingPoint!.Customer!.Name,
                    CustomerCode = collaborativeDemand.ShippingPoint!.Customer!.Code,
                    DistributionChannel = collaborativeDemand.ShippingPoint!.Customer!.DistributionChannel!.Name,
                    ShippingPointName = collaborativeDemand.ShippingPoint!.Name,
                    CityName = collaborativeDemand.ShippingPoint.City!.Name,
                    ProductName = collaborativeDemand.Product!.Name,
                    ProductCode = collaborativeDemand.Product.Code,
                    UserEmail = "waltermorales",
                    UserId = "1"
                };

                // Asignar las cantidades del diccionario a las columnas YearMonth
                foreach (var yearMonthQuantity in yearMonthQuantities)
                {
                    var yearMonthColumnName = "YM_" + yearMonthQuantity.Key;
                    collaborativeDemandDTO.GetType().GetProperty(yearMonthColumnName)?.SetValue(collaborativeDemandDTO, yearMonthQuantity.Value);
                }

                result.Add(collaborativeDemandDTO);
            }
            return result;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var collaborativeDemand = await _context.CollaborativeDemand
                .Include(cd => cd.CollaborativeDemandComponentsDetails)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (collaborativeDemand == null)
            {
                return NotFound();
            }

            return Ok(collaborativeDemand);
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
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
                                    CollaborativeDemandId = cd.Id,
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