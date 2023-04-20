using Clean.Core.Models.Api;
using Clean.Core.Models.Auth;
using Clean.Core.Models.Company;

namespace Clean.Infrastructure.Identity.Services
{
    public interface IUserService
    {
        Task<UserModel> GetByIdAsync(string userId);
        List<ApplicationUserModel> GetUsers();
        List<RoleModel> GetRoles();

        Task<Result> InsertAsync(UserInsertModel userModel);

        Task<Result> AddRoleAsync(string byUserId,UserRoleModel userModel);

        Task<Result> RemoveRoleAsync(string byUserId, UserRoleModel userModel);

        Result DisableUser(string byUserId, string userId);

        Task<Result> ResetPasswordAsync(string byUserId, string userId);
    }
}