﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class CollaborativeDemandsDemoController : GenericController<CollaborativeDemandDemo>
    {
        private readonly DataContext _context;
        private readonly IGenericUnitOfWork<CollaborativeDemandDemo> _unitOfWork;
        public CollaborativeDemandsDemoController(IGenericUnitOfWork<CollaborativeDemandDemo> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("totalPages")]
        public override async Task<ActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var queryable = _context.CollaborativeDemandDemo.AsQueryable();

            double count = await queryable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }
    }
}
