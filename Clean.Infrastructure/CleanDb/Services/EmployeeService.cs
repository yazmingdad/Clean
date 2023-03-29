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
        public List<CoreModel.Employee> GetAllLight(bool isRetired = false)
        {
            return (from employee in _cleanContext.Set<Employee>()
                    where employee.IsRetired == isRetired
                    select new CoreModel.Employee
                    {
                        Id = employee.Id,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,          
                    }).ToList();
        }
        public List<CoreModel.Employee> GetAll(bool isRetired = false)
        {
            var output = new List<CoreModel.Employee>();

            using (_cleanContext)
            {
                output = (from employee in _cleanContext.Set<Employee>()
                            where employee.IsRetired==isRetired
                            join rank in _cleanContext.Set<Rank>()
                                on employee.RankId equals rank.Id
                            join department in _cleanContext.Set<Department>()
                                on employee.DepartmentId equals department.Id
                            join activeCard in _cleanContext.Set<Card>()
                                on employee.ActiveCardId equals activeCard.Id into gj
                                from activeCard in gj.DefaultIfEmpty()                           
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
                                ActiveCard = Mapper.Map<CoreModel.Card>(activeCard),
                                Cards = Mapper.Map<List<CoreModel.Card>>((from card in _cleanContext.Set<Card>() where card.EmployeeId== employee.Id select card).ToList())
                            }).ToList();
        }

            return output;
        }

        public CoreModel.Employee GetById(int id)
        {
            return Mapper.Map<CoreModel.Employee>(_cleanContext.Employees.AsNoTracking().FirstOrDefault(x => x.Id == id)); 
        }

        public Result Update(CoreModel.Employee employee)
        {
            try
            {
                var employeeData = Mapper.Map<Employee>(employee);

                var existing = _cleanContext.Employees.FirstOrDefault(x => x.Id != employeeData.Id 
                                                                    && (x.SSN == employeeData.SSN || 
                                                                    (x.FirstName.ToLower() == employeeData.FirstName.ToLower() && x.LastName.ToLower() == employeeData.LastName.ToLower())));              

                if(existing != null)
                {
                    if(existing.SSN== employeeData.SSN)
                    {
                        throw new Exception("SSN for another employee");
                    }
                    throw new Exception("Name for another employee");
                    
                }

                _cleanContext.Employees.Attach(employeeData);
                _cleanContext.Entry(employeeData).State = EntityState.Modified;
                _cleanContext.SaveChanges();
                return new Result();
            }
            catch(Exception ex)
            {
                return new Result { IsFailure = true, Reason = ex.Message };
            }
        }
        public Result Insert(CoreModel.EmployeeInsert employee)
        {
            using var transaction = _cleanContext.Database.BeginTransaction();

            try
            {
                var existing = _cleanContext.Employees.FirstOrDefault(e=> (e.FirstName== employee.FirstName && e.LastName==employee.LastName) || e.SSN==employee.SSN);

                if(existing != null)
                {
                    throw new Exception("Employee already exists");
                }

                var existingsCard = _cleanContext.Cards.FirstOrDefault(c => c.Number == employee.CardNumber);

                if(existingsCard != null)
                {
                    throw new Exception("Card already exists");
                }

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
                return new Result { IsFailure = true, Reason=ex.Message};
            }
            return new Result();
        }

        public Result InsertCard(CoreModel.Card card)
        {
            try
            {
                var exists = _cleanContext.Cards.FirstOrDefault(c=>c.Number==card.Number);

                if(exists != null)
                {
                    throw new Exception("Card already exists");
                }

                var cardData = Mapper.Map<Card>(card);

                _cleanContext.Cards.Add(cardData);
                _cleanContext.SaveChanges();

                return new Result { Id = cardData.Id };


            }catch(Exception ex)
            {
                return new Result { IsFailure = true,Reason=ex.Message};
            }
        }
    }
}


