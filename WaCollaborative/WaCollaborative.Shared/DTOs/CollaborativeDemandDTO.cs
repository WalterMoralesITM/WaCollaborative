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
       public List<CollaborativeDemandComponentDTO> CollaborativeDemandComponentsDetails { get; set; }





    }
}
