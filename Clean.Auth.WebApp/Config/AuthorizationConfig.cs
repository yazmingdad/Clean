using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Clean.Auth.WebApp.Config
{
    public static class AuthorizationConfig
    {
        public static void SetAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(
                options =>
                {
                    options.AddPolicy(
                        JwtBearerDefaults.AuthenticationScheme,
                        new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                            .RequireAuthenticatedUser()
                            .Build());
                });
        }
    }
}
