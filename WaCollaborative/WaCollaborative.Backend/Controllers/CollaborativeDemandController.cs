﻿#region using

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers;
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

        public CollaborativeDemandController(IGenericUnitOfWork<CollaborativeDemand> unitOfWork, DataContext context, IUserHelper userHelper,IMailHelper mailHelper) : base(unitOfWork, context)
        {
            _context = context;
            _userHelper = userHelper;
            _mailHelper = mailHelper;
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
                        //UserEmail = collaborativeDemand.us,
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

        [AllowAnonymous]
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
                
                var fileDownloadName = $"CollaborativeDemands{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                                
                var excelFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "ExcelFiles", fileDownloadName);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                // Crear el archivo Excel
                using (var package = new ExcelPackage())
                {
                    // Crear la hoja de trabajo
                    var worksheet = package.Workbook.Worksheets.Add("CollaborativeDemands");
                    
                    var headers = result.First().GetType().GetProperties().Select(property => property.Name).ToArray();

                    for (var i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = headers[i];
                        worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    }                    

                    for (var i = 0; i < result.Count; i++)
                    {
                        var rowData = result[i];
                        var properties = rowData.GetType().GetProperties();
                        for (var j = 0; j < properties.Length; j++)
                        {
                            worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(rowData);
                        }
                    }


                    FileInfo excelFile = new FileInfo(excelFilePath);
                    package.SaveAs(excelFile);
                }
                                

                var downloadLink = Path.Combine("ExcelFiles", excelFilePath);             

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == User.Identity!.Name);
                if (user == null)
                {
                    return NotFound();
                }

                
                var emailSubject = "WaCollaborative - Descarga de archivo";
                var emailBody = $"<h1>WaCollaborative - Descarga de archivo</h1>" +
                                $"<p>Puedes descargar el archivo haciendo clic en el siguiente enlace:</p>" +
                                $"<b><a href={downloadLink}>Descargar Archivo</a></b>";
                
                var response = _mailHelper.SendMail(user.FullName,user.Email!,
                    emailSubject, emailBody);

                return Ok(new { ExcelLink = downloadLink });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ErrorMessage = ex.Message });
            }
        }       

    }
}