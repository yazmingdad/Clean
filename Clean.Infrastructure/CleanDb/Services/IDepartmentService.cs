using Clean.Core.Models.Api;
using Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface IDepartmentService
    {
        Department GetById(int id);
        List<Department> getAll();
        List<DepartmentType> getAllType();
        List<Department> getByIsDown(bool isDown);
        List<Department> getDown();

        Result Insert(DepartmentInsert department);
        Result Update(Department department);
    }
}