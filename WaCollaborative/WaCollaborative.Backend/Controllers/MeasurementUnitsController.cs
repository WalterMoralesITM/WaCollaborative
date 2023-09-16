using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementUnitsController : GenericController<MeasurementUnit>
    {
        DataContext _context;
        public MeasurementUnitsController(IGenericUnitOfWork<MeasurementUnit> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.MeasurementUnits
                .AsQueryable();

            var result = await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            MeasurementUnit? measurementUnit = await _context.MeasurementUnits
                .FirstOrDefaultAsync(c => c.Id == id);

            if (measurementUnit == null)
            {
                return NotFound();
            }
            return Ok(measurementUnit);
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.MeasurementUnits
                .AsQueryable();

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }
    }
}