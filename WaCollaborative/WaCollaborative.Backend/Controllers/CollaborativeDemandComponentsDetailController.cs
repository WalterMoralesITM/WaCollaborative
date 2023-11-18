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

namespace WaCollaborative.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("/api/[controller]")]
    public class CollaborativeDemandComponentsDetailController : GenericController<CollaborativeDemandComponentsDetail>//ControllerBase
    {
        
        private readonly DataContext _context;
        //private readonly IUserHelper _userHelper;

        public CollaborativeDemandComponentsDetailController(IGenericUnitOfWork<CollaborativeDemandComponentsDetail> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
            _context = context;            
        }
        //public CollaborativeDemandComponentsDetailController( DataContext context, IUserHelper userHelper) : base(unitOfWork, context)
        //{
        //    _context = context;
        //    _userHelper = userHelper;
        //}

        [HttpPut("full")]
        public override async Task<IActionResult> PutAsync(CollaborativeDemandComponentsDetail collaboration)
        {
            //var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
            //if (user == null)
            //{
            //    return NotFound();
            //}

            var collaborationDetail = await _context.CollaborativeDemandComponentsDetail
                .FirstOrDefaultAsync(s => s.Id == collaboration.Id);
            if (collaborationDetail == null)
            {
                return NotFound();
            }
            collaborationDetail.Quantity = collaboration.Quantity;  
            collaborationDetail.UpdateDate = DateTime.Now;
            _context.Update(collaborationDetail);
            await _context.SaveChangesAsync();
            return Ok(collaborationDetail);
        }
        
    }
}
