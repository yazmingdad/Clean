using Clean.Core.Models.Auth;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Clean.Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly CleanContext _cleanContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ApplicationDbContext context, CleanContext cleanContext, UserManager<ApplicationUser> userManager)
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
    }
}
