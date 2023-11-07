using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Shared.DTOs
{
    public class CollaborativeDemandComponentDTO : CollaborativeDemandComponentsDetail
    {
        //public decimal Quantity { get; set; }
        public int CollaborativeDemandId { get; set; }
        public string UserEmail { get; set; }
        public string UserId { get; set; }
        //public int YearMonth { get; set; }
    }
}
