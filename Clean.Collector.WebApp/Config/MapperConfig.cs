using AutoMapper;
using Clean.Infrastructure.CleanDb.Services;
using Clean.Infrastructure.Profile;

namespace Clean.Collector.WebApp.Config
{
    public static class MapperConfig
    {
        public static void SetupMapper(this IServiceCollection services, ConfigurationManager configuration)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CommonProfile());
            });

            AutoMapper.IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
