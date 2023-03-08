using Clean.Core.Models.Company;
using Clean.Infrastructure.CleanDb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Collector.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;

        private readonly IEmployeeService _employeeService;



        public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _employeeService =employeeService;

        }

        [HttpGet]
        public IEnumerable<Employee> Get(bool isRetired=false)
        {
            return _employeeService.getAll(isRetired);
        }

    }
}
