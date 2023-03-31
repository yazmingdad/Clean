
using AutoMapper;
using Clean.Core.Models.Api;
using Clean.Infrastructure.CleanDb.Models;
using Microsoft.EntityFrameworkCore;
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
        public CoreModel.Department GetById(int id)
        {
            return Mapper.Map<CoreModel.Department>(_cleanContext.Departments.AsNoTracking().FirstOrDefault(x => x.Id == id));
        }
        public List<CoreModel.Department> getAll()
        {
           return (from department in _cleanContext.Set<Department>() select Mapper.Map<CoreModel.Department>(department)).ToList();
        }

        public List<CoreModel.DepartmentType> getAllType()
        {
           return (from departmentType in _cleanContext.Set<DepartmentType>() select Mapper.Map<CoreModel.DepartmentType>(departmentType)).ToList();
        }

        public List<CoreModel.Department> getByIsDown(bool isDown)
        {

         var result = (from department in _cleanContext.Set<Department>()
                    where department.IsDown == isDown
                    join departmentType in _cleanContext.Set<DepartmentType>()
                    on department.DepartmentTypeId equals departmentType.Id
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

            var types = _cleanContext.DepartmentTypes.ToList();

            List<CoreModel.Department> output = new List<CoreModel.Department>();

            foreach (var type in types)
            {
                output.AddRange(result.Where(r => r.DepartmentTypeId == type.Id));
            }

            return output;

        }


        public List<CoreModel.Department> getDown()
        {
            throw new NotImplementedException();
        }

        public Result Insert(CoreModel.DepartmentInsert department)
        {
            try
            {
                var existing = _cleanContext.Departments.FirstOrDefault(d => d.Name == department.Name || d.ShortName == department.ShortName);

                if (existing != null)
                {
                    throw new Exception("Department already exists");
                }

                Department departmentData = Mapper.Map<Department>(department);

                _cleanContext.Departments.Add(departmentData);
                _cleanContext.SaveChanges();

            }
            catch(Exception ex)
            {
                return new Result { IsFailure = true, Reason = ex.Message };
            }


            return new Result();

        }

        public Result Update(CoreModel.Department department)
        {
            try
            {
                var departmentData = Mapper.Map<Department>(department);

                var existing = _cleanContext.Departments.FirstOrDefault(x => x.Id != departmentData.Id && (x.Name.ToLower() == departmentData.Name.ToLower() || x.ShortName.ToLower() == departmentData.ShortName.ToLower()));

                if (existing != null)
                {
                    if (existing.Name== departmentData.Name)
                    {
                        throw new Exception("Name for another department");
                    }
                    throw new Exception("ShortName for another employee");

                }

                _cleanContext.Departments.Attach(departmentData);
                _cleanContext.Entry(departmentData).State = EntityState.Modified;
                _cleanContext.SaveChanges();
                return new Result();
            }
            catch (Exception ex)
            {
                return new Result { IsFailure = true, Reason = ex.Message };
            }
        }
    }
}
