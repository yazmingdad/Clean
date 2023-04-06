using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Auth
{
   public class UserInsertModel
    {
        public string Id { get; set; }
        public int EmployeeId { get; set; }    
        public string RoleId { get; set; }
    }
}
