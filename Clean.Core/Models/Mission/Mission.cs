using Clean.Core.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Mission
{
    public class Mission
    {
        public int Id { get; set; }
        public Department Department { get; set; }
        public Status Status { get; set; }
        public City StartCity { get; set; }
        public Priority Priority { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Budget { get; set; }

        public int Cost { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int Distance { get; set; }

        public bool IsInCountry { get; set; } = true;

        public List<Employee> Participants { get; set; }

        public List<City> Destinations { get; set; }
    }
}
