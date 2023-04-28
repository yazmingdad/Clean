using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public class MissionDestination
    {
        public int Id { get; set; }
        public int MissionId { get; set; }
        public int DestinationId { get; set; }
        public int Order { get; set; }
    }
}
