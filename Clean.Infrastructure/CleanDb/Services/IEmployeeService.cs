using Clean.Core.Models.Api;
using Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface IEmployeeService
    {
        ApiResponse Insert(EmployeeInsert employee);

        List<Employee> getAll(bool isRetired = false);
    }
}