﻿#region Using

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Helpers;
using WaCollaborative.Backend.Interfaces;

#endregion Using

namespace WaCollaborative.Backend.Controllers
{

    /// <summary>
    /// The Controller DemandTypesController
    /// </summary>

    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class DemandTypesController : GenericController<DemandType>
    {

        #region Attributes

        private readonly IGenericUnitOfWork<DemandType> _unitOfWork;
        private readonly DataContext _context;

        #endregion Attributes

        #region Constructor

        public DemandTypesController(IGenericUnitOfWork<DemandType> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        #endregion Constructor

        #region Methods

        [AllowAnonymous]
        [HttpGet("combo/{eventTypeId:int}")]
        public async Task<ActionResult> GetComboAsync(int eventTypeId)
        {
            return Ok(await _context.DemandTypes                
                .OrderBy(s => s.Name)
                .ToListAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.DemandTypes                
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync());                       
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.Countries.AsQueryable();

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
            var demandType = await _context.DemandTypes                
                .FirstOrDefaultAsync(s => s.Id == id);
            if (demandType == null)
            {
                return NotFound();
            }
            return Ok(demandType);
        }

        #endregion Methods

    }
}