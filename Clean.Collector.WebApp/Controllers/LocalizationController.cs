using Clean.Core.Models.Company;
using Clean.Infrastructure.CleanDb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clean.Collector.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly ILogger<LocalizationController> _logger;

        private readonly ILocalizationService _localizationService;

        public LocalizationController(ILocalizationService localizationService, ILogger<LocalizationController> logger)
        {
            _logger = logger;
            _localizationService = localizationService;
        }


        [HttpGet]
        [Route("Cities")]
        public ActionResult<City> GetCities()
        {
            try
            {
                return Ok(_localizationService.getCities());
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
