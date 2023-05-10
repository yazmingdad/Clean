using Clean.Core.Models.Company;
using Clean.Core.Models.Mission;
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.CleanDb.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clean.Collector.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MissionController : ControllerBase
    {
        private readonly ILogger<MissionController> _logger;
        private readonly IMissionService _missionService;

        public MissionController(ILogger<MissionController> logger,IMissionService missionService)
        {
            _logger = logger;
            _missionService= missionService;
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        public IActionResult Insert(MissionInsert mission)
        {

            string byUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            mission.ByUserId= new Guid(byUserId);

            var output = _missionService.Insert(mission);

            if (output.IsFailure)
            {
                return BadRequest(output);
            }

            return Ok();
        }
    }
}
