using Clean.Core.Models.Api;
using Clean.Core.Models.Company;
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
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;

        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger)
        {
            _logger = logger;
            _departmentService = departmentService;
        }

        [HttpGet]
        public IEnumerable<Department> Get()
        {
           return _departmentService.getAll();
        }


        [HttpGet]
        [Route("Types")]
        public ActionResult<DepartmentType> GetTypes()
        {
            try
            {
                return Ok(_departmentService.getAllType());
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet]
        [Route("up")]
        public ActionResult<Department> GetUp()
        {
            try
            {
                return Ok(_departmentService.getByIsDown(false));
            }
            catch
            {
                return BadRequest();
            }
           
        }

        [HttpGet]
        [Route("down")]
        public ActionResult<Department> GetDown()
        {
            try
            {
                return Ok(_departmentService.getByIsDown(true));
            }
            catch
            {
                return BadRequest(new List<Department>());
            }
        }


        [HttpPost]
        [Authorize(Roles = "Config")]
        public IActionResult Insert(DepartmentInsert department)
        {
            var output = _departmentService.Insert(department);

            if (output.IsFailure)
            {
                return BadRequest(output);
            }

            return Ok();
        }

        [Authorize(Roles = "Config")]
        [HttpPatch("{id}")]
        public IActionResult Patch(int id,
          [FromBody] JsonPatchDocument<Department> patchDoc)
        {
            try
            {
                if (patchDoc != null)
                {
                    var department = _departmentService.GetById(id);

                    patchDoc.ApplyTo(department, ModelState);

                    if (!ModelState.IsValid)
                    {
                        throw new Exception("Invalid structure");
                    }

                    var output = _departmentService.Update(department);

                    if (output.IsFailure)
                    {
                        return BadRequest(output);
                    }

                    _logger.LogInformation("Departement Patch");

                    return Ok();
                }

                throw new Exception("Nothing to Patch");
            }
            catch (Exception ex)
            {
                return BadRequest(new Result { IsFailure = true, Reason = ex.Message });
            }

        }
    }
}
