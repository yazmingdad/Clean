using Clean.Core.Models.Api;
using Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface IEmployeeService
    {
        Employee GetById(int id);
        Result Insert(EmployeeInsert employee);
        Result Update(Employee employee);
        List<Employee> GetAll(bool isRetired = false);
    }
}