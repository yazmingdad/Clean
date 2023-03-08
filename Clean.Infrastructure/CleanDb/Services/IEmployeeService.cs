using Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface IEmployeeService
    {
        List<Employee> getAll(bool isRetired = false);
    }
}