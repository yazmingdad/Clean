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
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;

        private readonly IEmployeeService _employeeService;

        public CardController(IEmployeeService employeeService, ILogger<CardController> logger)
        {
            _logger = logger;
            _employeeService = employeeService;

        }

        [HttpPost]
        public IActionResult Insert(Card card)
        {
            var output = _employeeService.InsertCard(card);

            if (output.IsFailure)
            {
                return BadRequest(output);
            }

            return Ok(output);
        }
    }
}
