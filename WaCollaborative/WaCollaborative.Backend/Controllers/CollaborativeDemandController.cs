﻿#region using

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
            string name = User != null ? User!.Identity!.Name! : string.Empty;
            var user = await _userHelper.GetUserAsync(name);
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
                .ThenInclude(ir => ir!.CollaborationCalendars)
                .FirstOrDefaultAsync(x => x.Email == name);

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

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            return Ok(await queryable
                .OrderBy(c => c.Product!.Name)
                .Paginate(pagination)
                .ToListAsync());
        }


        [HttpGet("All")]
        public async Task<IActionResult> GetAllAsync()
        {
            string name = User != null ? User.Identity!.Name! : string.Empty;
            var user = await _userHelper.GetUserAsync(name);

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


        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            string name = User != null ? User.Identity!.Name! : string.Empty;
            var user = await _userHelper.GetUserAsync(name);

            if (user == null)
            {
                return BadRequest("User not valid.");
            }

            var queryable = _context.CollaborativeDemand
                .Include(c => c.Product)
                .Include(c => c.ShippingPoint)
                .Include(c => c.CollaborativeDemandUsers)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Product!.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }
            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);

        }


        [HttpGet("ExcelGenerate")]
        public async Task<IActionResult> GetAsync([FromServices] IWebHostEnvironment? webHostEnvironment)
        {
            string name = User != null ? User.Identity!.Name! : string.Empty;
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

            var user = await _userHelper.GetUserAsync(name);

            if (user == null)
            {
                return NotFound();
            }

            excelFilePath = excelFilePath == null ? "C:/Projects/WaCollaborative/WaCollaborative/WaCollaborative.UnitTest/Controllers/Test.xlsx" : excelFilePath;

            using (var stream = new MemoryStream(System.IO.File.ReadAllBytes(excelFilePath)))
            {
                var response = await _mailHelper.SendMailWithAttachmentAsync(user.FullName, user.Email!, "WaCollaborative - Descarga de archivo", "Adjunto encontrarás el archivo de Excel.", stream, fileDownloadName);
            }

            DeletedField(excelFilePath);

            return Ok(new { Message = "Archivo enviado por correo electrónico." });
        }

        private void DeletedField(string excelFilePath)
        {
            if (excelFilePath != "C:/Projects/WaCollaborative/WaCollaborative/WaCollaborative.UnitTest/Controllers/Test.xlsx")
            {
                System.IO.File.Delete(excelFilePath);
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
    }
}