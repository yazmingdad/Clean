using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Company
{
    public class Department
    {
        public int Id { get; set; }
        public int DepartmentTypeId { get; set; }
        public int? ParentId { get; set; }
        public int? ManagerId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public bool IsDown { get; set; }
    }
}
