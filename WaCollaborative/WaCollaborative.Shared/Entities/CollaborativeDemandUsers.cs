using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaCollaborative.Shared.Entities
{
    public class CollaborativeDemandUsers
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public string UserId { get; set; }

        public CollaborativeDemand? CollaborativeDemand { get; set; }
        public int CollaborativeDemandId { get; set; }
    }
}
