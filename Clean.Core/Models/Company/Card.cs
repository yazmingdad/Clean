using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Company
{
    public class Card
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }
    }
}
