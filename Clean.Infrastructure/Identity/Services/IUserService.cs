using Clean.Core.Models;

namespace Clean.Infrastructure.Identity.Services
{
    public interface IUserService
    {
        Task<UserModel> GetByIdAsync(string userId);
    }
}