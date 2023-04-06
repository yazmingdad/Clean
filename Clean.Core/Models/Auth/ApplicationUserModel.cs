using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Core.Models.Auth
{
    public class ApplicationUserModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public List<RoleModel> Roles { get; set; }=new List<RoleModel>();
    }


}
