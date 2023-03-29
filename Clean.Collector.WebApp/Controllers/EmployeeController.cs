using Castle.Core.Resource;
using Clean.Core.Models.Api;
using Clean.Core.Models.Company;
using Clean.Infrastructure.CleanDb.Seed;
using Clean.Infrastructure.CleanDb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
            return _employeeService.GetAll(isRetired);
        }

        [HttpGet]
        [Route("Light")]
        public ActionResult<Employee> GetLight(bool isRetired = false)
        {

            try
            {
                return Ok(_employeeService.GetAllLight(isRetired));
            }
            catch
            {
                return BadRequest();
            }
          
        }


        [HttpPost]
        [Authorize(Roles = "Config")]
        public IActionResult Insert(EmployeeInsert employee)
        { 
            var output = _employeeService.Insert(employee);

            if(output.IsFailure)
            {
                return BadRequest(output);
            }

            return Ok();
        }

        [Authorize(Roles = "Config")]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id,
            [FromBody] JsonPatchDocument<Employee> patchDoc)
        {
            try
            {
                if (patchDoc != null)
                {
                    var employee = _employeeService.GetById(id);

                    patchDoc.ApplyTo(employee, ModelState);

                    if (!ModelState.IsValid)
                    {
                        throw new Exception("Invalid structure");
                    }

                    var output = _employeeService.Update(employee);

                    if (output.IsFailure)
                    {
                        return BadRequest(output);
                    }

                    _logger.LogInformation("Employee Patch");

                    return Ok();
                }

                throw new Exception("Nothing to Patch");
            }
            catch(Exception ex)
            {
                return BadRequest(new Result { IsFailure = true, Reason = ex.Message });
            }

        }

       

    }
}
