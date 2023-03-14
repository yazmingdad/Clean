using AutoMapper;
using Clean.Infrastructure.CleanDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreModel = Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public class DepartmentService : ServiceBase, IDepartmentService
    {
        public DepartmentService(IMapper mapper, CleanContext cleanContext) : base(mapper, cleanContext)
        {
        }
        public List<CoreModel.Department> getAll()
        {
            //using (_cleanContext)
            //{
                return (from department in _cleanContext.Set<Department>() select Mapper.Map<CoreModel.Department>(department)).ToList();
            //}
        }
    }
}
