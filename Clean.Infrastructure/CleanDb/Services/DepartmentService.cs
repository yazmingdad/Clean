
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

        public List<CoreModel.DepartmentType> getAllType()
        {
           return (from departmentType in _cleanContext.Set<DepartmentType>() select Mapper.Map<CoreModel.DepartmentType>(departmentType)).ToList();
        }

        public List<CoreModel.Department> getByType(string type)
        {
            var departmentType = _cleanContext.DepartmentTypes.FirstOrDefault(t => t.Name == type);

            if(departmentType == null) 
            {
                throw new Exception("Unknown Error");
            }

            return (from department in _cleanContext.Set<Department>()
                    where department.DepartmentTypeId==departmentType.Id && !department.IsDown
                    join city in _cleanContext.Set<City>()
                    on department.CityId equals city.Id
                    join employee in _cleanContext.Set<Employee>()
                    on department.ManagerId equals employee.Id into gj
                    from employee in gj.DefaultIfEmpty()
                    join parent in _cleanContext.Set<Department>()
                    on department.ParentId equals parent.Id into gp
                    from parent in gp.DefaultIfEmpty()
                    select new CoreModel.Department
                    {
                        Id=department.Id,
                        Name=department.Name,
                        ShortName=department.ShortName,
                        ParentId=department.ParentId,
                        ManagerId=department.ManagerId,
                        DepartmentTypeId=department.DepartmentTypeId,
                        CityId=department.CityId,
                        IsDown=department.IsDown,
                        DepartmentType= new CoreModel.DepartmentType
                        {
                            Id=departmentType.Id,
                            Name=departmentType.Name,
                        },
                        City= new CoreModel.City
                        {
                            Id=city.Id,
                            Name=city.Name
                        },
                        Manager=new CoreModel.Employee
                        {
                            Id=employee.Id,
                            FirstName=employee.FirstName,
                            LastName=employee.LastName,
                            Avatar=employee.Avatar,
                        },
                        Parent=new CoreModel.Department
                        {
                            Id=parent.Id,
                            Name=parent.Name,
                            ShortName=parent.ShortName,
                        }

                    }).ToList();


        }


        public List<CoreModel.Department> getDown()
        {
            throw new NotImplementedException();
        }
    }
}
