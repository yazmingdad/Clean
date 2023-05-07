using Clean.Core.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Mission
{
    public class MissionInsert
    {
        public int DepartmentId { get; set; }
        public int StatusId { get; set; }
        public int StartCityId { get; set; }
        public Guid ByUserId { get; set; }
        public Priority Priority { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Budget { get; set; }
        public int Cost { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Distance { get; set; }
        public bool IsInCountry { get; set; } = true;
        public List<int> Destinations { get; set; }
        public List<int> Participants { get; set; }
    }
}
