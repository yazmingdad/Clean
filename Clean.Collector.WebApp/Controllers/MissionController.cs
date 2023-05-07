using Clean.Core.Models.Company;
using Clean.Core.Models.Mission;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.CleanDb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Collector.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MissionController : ControllerBase
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IMissionService _missionService;

        public MissionController(ILogger<MissionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public IActionResult Insert(MissionInsert mission)
        {
            var output = _missionService.Insert(mission);

            if (output.IsFailure)
            {
                return BadRequest(output);
            }

            return Ok();
        }
    }
}
