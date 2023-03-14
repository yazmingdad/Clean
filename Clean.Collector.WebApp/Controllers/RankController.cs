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
    public class RankController : ControllerBase
    {
        private readonly ILogger<RankController> _logger;

        private readonly IRankService _rankService;

        public RankController(IRankService rankService, ILogger<RankController> logger)
        {
            _logger = logger;
            _rankService = rankService;

        }

        [HttpGet]
        public IEnumerable<Rank> Get()
        {
            return _rankService.getAll();
        }
    }
}
