using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Shared.DTOs
{
    public class CollaborativeDemandDTO : CollaborativeDemand
    {
        public int CollaborativeDemandId { get; set; }
        public string? CustomerCode { get; set; }
        public string? CustomerName { get; set; }        
        public string? CityName { get; set; }
        public string? DistributionChannel { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? ShippingPointName { get; set; }
        public decimal Quantity { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        public int YearMonth { get; set; }
        public List<CollaborativeDemandComponentsDetail> CollaborativeDemandComponentsDetailsDTO { get; set; }

    }
}
