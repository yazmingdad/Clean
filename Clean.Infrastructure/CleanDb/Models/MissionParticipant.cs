using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public class MissionParticipant
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int MissionId { get; set; }
    }
}
