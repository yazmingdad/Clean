﻿using Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface IDepartmentService
    {
        List<Department> getAll();
        List<Department> getByType(string type);
        List<Department> getDown();
    }
}