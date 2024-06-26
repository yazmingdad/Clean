﻿using Clean.Core.Models.Api;
using Clean.Core.Models.Auth;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Email;
using Clean.Infrastructure.Identity.Models;
using Clean.Infrastructure.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Clean.Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly CleanContext _cleanContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMailService _mailService;



        public UserService( IMailService mailService, ApplicationDbContext context, CleanContext cleanContext, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _cleanContext = cleanContext;
            _userManager = userManager;
            _mailService= mailService;

        }

        public async Task<Result> AddRoleAsync(string byUserId,UserRoleModel pairing)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == pairing.UserId);

              
                if (user == null || (byUserId == user.Id))
                {
                    throw new Exception("User Not Found");
                }

                await _userManager.AddToRoleAsync(user, pairing.RoleName);

                _context.SaveChanges();

            }
            catch(Exception ex)
            {
                return new Result { IsFailure= true };
            }

            return new Result();
        }

        public Result DisableUser(string byUserId, string userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);


                if (user == null || (byUserId == user.Id) || user.UserName.ToLower()=="cleaner")
                {
                    throw new Exception("User Not Found");
                }

                user.IsDown = true;

                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return new Result { IsFailure = true };
            }

            return new Result();
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

        public async Task<Result> InsertAsync(UserInsertModel userModel)
        {
            try
            {
                EmailMessage message = new();

                var employee = _cleanContext.Employees.FirstOrDefault(e => e.Id == userModel.EmployeeId);

                if (employee == null)
                {
                    throw new Exception("Couldn't Find Employee");
                }

                var role = _context.Roles.FirstOrDefault(r => r.Name == userModel.RoleName);

                if(role == null)
                {
                    throw new Exception("Role Not Found");
                }

                var guid = Guid.NewGuid();

                var user = _context.Users.FirstOrDefault(u => u.Id.ToLower()==guid.ToString().ToLower());

                if( user != null)
                {
                    throw new Exception("User already exists");
                }

                var username = $"{employee.FirstName.Substring(0, 1)}.{employee.LastName.Replace(" ", String.Empty)}".ToLower();

                user = await _userManager.FindByNameAsync(username);

                if(user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = username,
                        Email = "ygabdelo@gmail.com",
                        NormalizedUserName = username.ToUpper(),
                        NormalizedEmail= "ygabdelo@gmail.com".ToUpper(),
                    };

                    PasswordGenerator generator = new PasswordGenerator();
                    string password = generator.Generate();

                    message = EmailBuilder.NewUserAssigned(employee, username, password);

                    var hashedPassword = _userManager.PasswordHasher.HashPassword(user, password);

                    user.Id = guid.ToString().ToLower();
                    user.ConcurrencyStamp = Guid.NewGuid().ToString();
                    user.SecurityStamp = Guid.NewGuid().ToString("D").ToUpper();
                    user.PasswordHash = hashedPassword;

                    object[] paramItems = new object[]
                        {
                                            new SqlParameter("@uid", user.Id),

                                            new SqlParameter("@un", user.UserName),
                                            new SqlParameter("@nun", user.NormalizedUserName),

                                            new SqlParameter("@em", user.Email),
                                            new SqlParameter("@nem", user.NormalizedEmail),

                                            new SqlParameter("@ph", user.PasswordHash),
                                            new SqlParameter("@cs", user.ConcurrencyStamp),
                                            new SqlParameter("@ss", user.SecurityStamp),

                                            new SqlParameter("@roleId", role.Id),


                                            new SqlParameter("@cleanUserId", guid.ToString().ToUpper()),
                                            new SqlParameter("@employeeId", employee.Id)
                        };


                    _context.Database.ExecuteSqlRaw(@$"BEGIN TRANSACTION 
                                                             BEGIN TRY  
                                                              INSERT INTO CleanIdentity.dbo.AspNetUsers(Id,IsDown,IsFirstLogin,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnabled,AccessFailedCount)
                                                              VALUES(@uid,0,1,@un,@nun,@em,@nem,1,@ph,@ss,@cs,0,0,1,0);

		                                                      INSERT INTO CleanIdentity.dbo.AspNetUserRoles(UserId,RoleId)
		                                                      VALUES(@uid,@roleId)

                                                              INSERT INTO Clean.dbo.Users 
                                                              VALUES(@cleanUserId,@employeeId);
                                                              COMMIT;
                                                        
                                                             END TRY  
                                                             BEGIN CATCH  
                                                              ROLLBACK; 
                                                              THROW; 
                                                             END CATCH;   
                                                  ", paramItems);

                }
                else
                {
                    if (user.IsDown)
                    {
                        // Activate the user
                        user.IsDown = false;

                        //Add role to the User 
                        await _userManager.AddToRoleAsync(user, userModel.RoleName);

                        //Configure the Email 
                        message = EmailBuilder.EnabledUser(employee, username);

                    }
                }




                message.Destination = user.Email;
                await _mailService.SendEmailAsync(message);
                return new Result();

            }
            catch(Exception ex)
            {
                return new Result { IsFailure= true };
            }

            

        }

        public async Task<Result> RemoveRoleAsync(string byUserId, UserRoleModel pairing)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == pairing.UserId);


                if (user == null || (byUserId == user.Id))
                {
                    throw new Exception("User Not Found");
                }

                await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);


                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                return new Result { IsFailure = true };
            }

            return new Result();
        }

        public async Task<Result> ResetPasswordAsync(string byUserId, string userId)
        {
            try
            {
                EmailMessage message = new();

                //Retrieve the Admin User account 
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);



                if (user == null || (byUserId == user.Id) || user.UserName.ToLower() == "cleaner")
                {
                    throw new Exception("User Not Found");
                }



                //Generate a Token
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                //Generate a password
                PasswordGenerator generator = new PasswordGenerator();
                string password = generator.Generate();

                message = EmailBuilder.PasswordChanged(user.UserName, password);

                //Reset the password
                await _userManager.ResetPasswordAsync(user, resetToken, password);

                //Set IsFirstLogin to true

                user.IsFirstLogin = true;

                _context.SaveChanges();

                message.Destination = user.Email;
                await _mailService.SendEmailAsync(message);

            }
            catch (Exception ex)
            {
                return new Result { IsFailure = true };
            }
            return new Result();
        }

        public async Task<Result> SetPasswordAsync(string userId,PasswordSetModel model)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if(user == null)
                {
                    throw new Exception("User Not Found");
                }

                if(user.IsFirstLogin)
                {
                    string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Reset the password
                     var result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

                    if (!result.Succeeded)
                    {
                        throw new Exception("Password Reset Failed");
                    }

                    user.IsFirstLogin = false;
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    //change the password
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword,
                                               model.Password);

                    if (!result.Succeeded)
                    {
                        throw new Exception("Password Reset Failed");
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                return new Result { IsFailure = true ,Reason= "Password Reset Failed" };
            }

            return new Result();
        }
    }
}
