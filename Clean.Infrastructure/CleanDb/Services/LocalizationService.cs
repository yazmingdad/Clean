using AutoMapper;
using Clean.Infrastructure.CleanDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreModel = Clean.Core.Models.Company;

namespace Clean.Infrastructure.CleanDb.Services
{
    public class LocalizationService : ServiceBase, ILocalizationService
    {
        public LocalizationService(IMapper mapper, CleanContext cleanContext) : base(mapper, cleanContext)
        {
        }

        public List<CoreModel.City> getCities()
        {
            return (from city in _cleanContext.Set<City>()
                    join country in _cleanContext.Set<Country>()
                    on city.CountryId equals country.Id
                    where country.Name == "Morocco"
                    select Mapper.Map<CoreModel.City>(city)).ToList();
        }

    }
}
