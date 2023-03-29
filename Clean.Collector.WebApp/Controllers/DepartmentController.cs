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
        [Route("Central")]
        public ActionResult<Department> GetCentral()
        {
            try
            {
                return Ok(_departmentService.getByType("Central"));
            }
            catch
            {
                return BadRequest();
            }
           
        }

        [HttpGet]
        [Route("Regional")]
        public ActionResult<Department> GetRegional()
        {
            try
            {
                return Ok(_departmentService.getByType("Regional"));
            }
            catch
            {
                return BadRequest(new List<Department>());
            }
        }

        [HttpGet]
        [Route("Provincial")]
        public ActionResult<Department> GetProvincial()
        {
            try
            {
                return Ok(_departmentService.getByType("Provincial"));
            }
            catch
            {
                return BadRequest(new List<Department>());
            }
        }

        //[HttpPost]
        //[Authorize(Roles = "Config")]
        //public IActionResult Insert(Department department)
        //{
        //    var output = _departmentService.Insert(department);

        //    if (output.IsFailure)
        //    {
        //        return BadRequest(output);
        //    }

        //    return Ok();
        //}
    }
}
