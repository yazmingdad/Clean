using Clean.Core.Models;
using Clean.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Clean.Infrastructure.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
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

            return new UserModel
            {
                Username = user.UserName,
                RoleNames = roles.ToList(),
                IsDown = user.IsDown,
                IsFirstLogin = user.IsFirstLogin,
            };
        }
    }
}
