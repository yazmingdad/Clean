using AutoMapper;
using Clean.Core.Models.Api;
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
                                Department = new CoreModel.Department
                                {
                                    Id=department.Id,
                                    Name=department.Name,
                                    ShortName= department.ShortName,                                     
                                },
                                Rank = Mapper.Map<CoreModel.Rank>(rank),
                                ActiveCard = Mapper.Map<CoreModel.Card>(card)
                            }).ToList();
        }

            return output;
        }

        public ApiResponse Insert(CoreModel.EmployeeInsert employee)
        {
            using var transaction = _cleanContext.Database.BeginTransaction();

            try
            {

                 Employee employeeData = Mapper.Map<Employee>(employee);
                 
                 _cleanContext.Employees.Add(employeeData);
                _cleanContext.SaveChanges();

                if (employeeData.Id == 0)
                {
                    throw new Exception("Employee Insert failed");
                }

                Card cardData = new Card
                {
                    EmployeeId= employeeData.Id,
                    Number = employee.CardNumber
                };

                _cleanContext.Cards.Add(cardData);
                _cleanContext.SaveChanges();

                if (cardData.Id == 0)
                {
                    throw new Exception("Card Insert failed");
                }

                employeeData.ActiveCardId=cardData.Id;
                _cleanContext.Update(employeeData);


                _cleanContext.SaveChanges();
                // Commit transaction if all commands succeed, transaction will auto-rollback
                // when disposed if either commands fails

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                // TODO: Handle failure
                return new ApiResponse { IsFailure = true, Reason=ex.Message};
            }
            return new ApiResponse();
        }
    }
}


