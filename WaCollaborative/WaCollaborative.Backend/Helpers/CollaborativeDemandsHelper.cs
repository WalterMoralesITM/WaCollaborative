using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Responses;

namespace WaCollaborative.Backend.Helpers
{
    public class CollaborativeDemandsHelper : ICollaborativeDemandsHelper
    {
        private readonly DataContext _context;

        public CollaborativeDemandsHelper(DataContext context)
        {
            _context = context;
        }
        //public async Task<Response<bool>> SynchronizeCollaborativeDemandsAsync()
        //{
        //    var query = from p in _context.Portfolios
        //                join pd in _context.PortfolioProducts on p.Id equals pd.PortfolioId
        //                join cu in _context.PortfolioCustomers on p.Id equals cu.PortfolioId
        //                join s in _context.ShippingPoints on cu.Id equals s.CustomerId
        //                join c in _context.CollaborativeDemand on new { ProductId = pd.ProductId, ShippingPointId = s.Id } 
        //                    equals new { ProductId = c.ProductId, ShippingPointId = c.ShippingPointId } into temp
        //                from cTemp in temp.DefaultIfEmpty()
        //                where cTemp == null
        //                select new
        //                {
        //                    DemandTypeId = 1,
        //                    EventTypeId = 1,
        //                    ProductId = pd.ProductId,
        //                    ShippingPointId = s.Id,
        //                    StatusId = 1
        //                };

        //    // Insertar los datos en CollaborativeDemand si no existen
        //    foreach (var item in query)
        //    {
        //        _context.CollaborativeDemand.Add(new CollaborativeDemand
        //        {
        //            DemandTypeId = item.DemandTypeId,
        //            EventTypeId = item.EventTypeId,
        //            ProductId = item.ProductId,
        //            ShippingPointId = item.ShippingPointId,
        //            StatusId = item.StatusId
        //        });
        //    }

        //    await _context.SaveChangesAsync();
        //    var response = new Response<bool>() { WasSuccess = true };
        //    return response;
        //}
        public async Task<Response<bool>> SynchronizeCollaborativeDemandsAsync()
        {
            var query = from pt in _context.Portfolios
                        join pp in _context.PortfolioProducts on pt.Id equals pp.PortfolioId
                        join pc in _context.PortfolioCustomers on pt.Id equals pc.PortfolioId
                        join s in _context.ShippingPoints on pc.CustomerId equals s.CustomerId                       
                        join c in _context.CollaborativeDemand on pp.ProductId equals c.ProductId
                            into cGroup
            from c in cGroup.DefaultIfEmpty()
                        join dt in _context.CollaborativeDemandComponentsDetail on c.Id equals dt.CollaborativeDemandId
                            into dtGroup
                        where c == null
                        select new CollaborativeDemand
                        {
                            DemandTypeId = 1,
                            EventTypeId = 1,
                            ProductId = pp.ProductId,
                            ShippingPointId = s.Id,
                            StatusId = 1
                        };

            var nuevosRegistros = query.ToList();

            _context.CollaborativeDemand.AddRange(nuevosRegistros);
            await _context.SaveChangesAsync();
            var response = new Response<bool>() { WasSuccess = true };
            return response;
        }
    }
}
