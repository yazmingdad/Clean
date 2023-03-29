using Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public interface ILocalizationService
    {
        List<City> getCities();
    }
}