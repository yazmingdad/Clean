using AutoMapper;
using Clean.Infrastructure.CleanDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using CoreModel = Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public class EmployeeService : ServiceBase, IEmployeeService
    {
        public EmployeeService(IMapper mapper,CleanContext cleanContext) : base(mapper,cleanContext)
        {
        }

        public List<CoreModel.Employee> getAll(bool isRetired = false)
        {
            var output = new List<CoreModel.Employee>();


            using (_cleanContext)
            {
                output = (from employee in _cleanContext.Set<Employee>()
                            join rank in _cleanContext.Set<Rank>()
                                on employee.RankId equals rank.Id
                            join department in _cleanContext.Set<Department>()
                                on employee.DepartmentId equals department.Id
                            join card in _cleanContext.Set<Card>()
                                on employee.ActiveCardId equals card.Id into gj
                                from card in gj.DefaultIfEmpty()                           
                            select new CoreModel.Employee
                            {
                                Id = employee.Id,
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                SSN = employee.SSN,
                                Avatar = employee.Avatar,
                                IsRetired = employee.IsRetired,
                                ActiveCard = Mapper.Map<CoreModel.Card>(card),
                                Department = new CoreModel.Department
                                {
                                    Id=department.Id,
                                    Name=department.Name,
                                    ShortName= department.ShortName,                                     
                                },
                                Rank = Mapper.Map<CoreModel.Rank>(rank)
                            }).ToList();


                

        }

            return output;
        }
    }
}


