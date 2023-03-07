using Clean.Core.Models.Auth;

namespace Clean.Infrastructure.Identity.Services
{
    public interface IUserService
    {
        Task<UserModel> GetByIdAsync(string userId);
    }
}