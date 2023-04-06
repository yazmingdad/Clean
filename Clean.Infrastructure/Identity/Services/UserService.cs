using Clean.Core.Models.Api;
using Clean.Core.Models.Auth;
using Clean.Email;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Clean.Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly CleanContext _cleanContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IMailService mailService, ApplicationDbContext context, CleanContext cleanContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _cleanContext = cleanContext;
            _userManager = userManager;
        }

        public async Task<UserModel> GetByIdAsync(string userId)
        {
           
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            

            if (user.IsDown)
            {
                return new UserModel
                {
                    IsDown = true,
                };
            }

            UserModel output = new UserModel
            {
                RoleNames = roles.ToList(),
                IsDown = user.IsDown,
                IsFirstLogin = user.IsFirstLogin,
            };

            using (_cleanContext)
            {
                var cleanUser = _cleanContext.Users
                    .Where(u => u.Id.Equals(new Guid(user.Id)))
                    .Join(_cleanContext.Employees, 
                          user => user.EmployeeId,        
                          employee => employee.Id,  
                          (user, employee) => new { User = user, Employee = employee })
                    .FirstOrDefault();

                if(cleanUser == null)
                {
                    return new UserModel
                    {
                        IsDown = true,
                    };
                }

                output.Username = $"{cleanUser.Employee.FirstName} {cleanUser.Employee.LastName}";
                output.Avatar = cleanUser.Employee.Avatar;
                
            }

            return output;
        }

        public List<RoleModel> GetRoles()
        {
            return _context.Roles.Select(r=> new RoleModel { Id = r.Id,Name = r.Name }).ToList();
        }

        public List<ApplicationUserModel> GetUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = _context.Users.Where(u => !u.IsDown).ToList();

            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

           
           
     

            foreach (var user in users)
            {

                var employee = (from us in _cleanContext.Set<User>()
                                 where (us.Id.Equals(new Guid(user.Id)))
                                 join e in _cleanContext.Set<Employee>()
                                 on us.EmployeeId equals e.Id
                                 select e
                                 ).FirstOrDefault();


                if(employee == null)
                {
                    throw new Exception("Employee not found");
                }              



                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                   FullName = $"{employee.FirstName} {employee.LastName}"
                };
                var roles = _context.Roles.ToList();

               foreach(var role in roles)
                {
                    var exists = userRoles.FirstOrDefault(ur=>ur.RoleId== role.Id && ur.UserId==user.Id);

                    if(exists == null)
                    {
                        u.Roles.Add(new RoleModel
                        {
                            Id = role.Id,
                            Name = role.Name,
                            IsEnabled = false
                        });
                    }
                    else
                    {
                        u.Roles.Add(new RoleModel
                        {
                            Id = role.Id,
                            Name = role.Name,
                            IsEnabled = true
                        });
                    }
                }

                output.Add(u);

            }

            return output;
        }

        public Result Insert(UserInsertModel userModel)
        {
            using (_cleanContext)
            {
                using (var transaction = _cleanContext.Database.BeginTransaction())
                {
                    try
                    {
                        // your transactional code
                        var employee = _cleanContext.Employees.FirstOrDefault(e => e.Id == userModel.EmployeeId);

                        if (employee == null)
                        {
                            throw new Exception("Couldn't Find Employee");
                        }      

                        using (_context)
                        {
                            _context.Database.UseTransaction(transaction.GetDbTransaction());

                            // your transactional code

                            ApplicationUser user = new ApplicationUser
                            {
                                UserName= $"{employee.FirstName.Substring(0, 1)}.{employee.LastName.Replace(" ", String.Empty)}".ToLower(),
                                Email= "ygabdelo@gmail.com",
                                IsDown = false,
                                IsFirstLogin= true,
                                
                            };
                            _context.SaveChanges();
                        }
                       
                        _cleanContext.SaveChanges();
                        // Commit transaction if all commands succeed, transaction will auto-rollback when disposed if either commands fails
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        // handle exception
                    }
                }

                return new Result();
            }
        }
    }
}
