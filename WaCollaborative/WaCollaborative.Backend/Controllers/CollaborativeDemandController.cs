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
using WaCollaborative.Shared.Helpers;

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
        private readonly IMailHelper _mailHelper;
        private readonly IExcelGenerator _excelGenerator;
        private readonly IGenericUnitOfWork<CollaborativeDemand> _unitOfWork;

        public CollaborativeDemandController(IGenericUnitOfWork<CollaborativeDemand> unitOfWork, DataContext context, IUserHelper userHelper, IMailHelper mailHelper, IExcelGenerator excelGenerator) : base(unitOfWork, context)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
            _excelGenerator = excelGenerator;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("UserCalendar")]
        public async Task<ActionResult> GetAsync()
        {
            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
            var userCalendar = new UserCalendarDTO();
            if (user!.UserType == UserType.Planner)
            {
                userCalendar.Role = user.UserType.ToString();
                userCalendar.CollaboartionEndDate = DateTime.Now;
                return Ok(userCalendar);
            }
            else
            {
                user = await _context.Users
                .Include(u => u.InternalRole)
                .ThenInclude(ir => ir.CollaborationCalendars)
                .FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);

                var internalRole = user?.InternalRole;
                var collaborationEnd = internalRole?.CollaborationCalendars?.FirstOrDefault()!.EndDate;

                userCalendar.Role = user!.UserType.ToString();
                userCalendar.CollaboartionEndDate = (DateTime)collaborationEnd!;
            }

            return Ok(userCalendar);
        }

        //portafolioIndex
        [HttpGet("Portfolio")]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.CollaborativeDemand
                .Include(c => c.Product)
                .Include(c => c.ShippingPoint)
                .Include(c => c.CollaborativeDemandUsers)
                .AsQueryable();

            return Ok(await queryable
                .OrderBy(c => c.Product!.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var user = await _userHelper.GetUserAsync(User.Identity!.Name!);

                var queryable = _context.CollaborativeDemand
                    .Include(cd => cd.Product)
                    .Include(cd => cd.ShippingPoint)
                    .ThenInclude(sp => sp!.City)
                    .Include(cd => cd.ShippingPoint)
                    .ThenInclude(sp => sp!.Customer)
                    .ThenInclude(c => c!.DistributionChannel)
                    .Include(cd => cd.CollaborativeDemandComponentsDetails)
                    .Where(cd => cd.CollaborativeDemandComponentsDetails!.Any(d => d.UserId == user.Id))
                    .AsQueryable();

                var collaborativeDemands = await queryable.ToListAsync();

                var result = FlattenCollaborativeDemands(collaborativeDemands);

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
                        CollaborativeDemandDetailId = detail.Id,
                        YearMonth = detail.YearMonth,
                        Quantity = detail.Quantity
                    };

                    result.Add(collaborativeDemandDTO);
                }
            }

            return result;
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
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

        [HttpGet("ExcelGenerate")]
        public async Task<IActionResult> GetAsync([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            try
            {
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

                var (excelFilePath, fileDownloadName) = await _excelGenerator.GenerateExcelFileAsync(FlattenCollaborativeDemands(collaborativeDemands));

                var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
                if (user == null)
                {
                    return NotFound();
                }

                using (var stream = new MemoryStream(System.IO.File.ReadAllBytes(excelFilePath)))
                {
                    var response = await _mailHelper.SendMailWithAttachmentAsync(user.FullName, user.Email!, "WaCollaborative - Descarga de archivo", "Adjunto encontrarás el archivo de Excel.", stream, fileDownloadName);
                }

                System.IO.File.Delete(excelFilePath);

                return Ok(new { Message = "Archivo enviado por correo electrónico." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }
        }
    }
}