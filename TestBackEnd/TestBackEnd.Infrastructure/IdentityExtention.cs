using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace TestBackEnd.Infrastructure
{
    /// <summary>
    /// Clase de configuracion del identity
    /// </summary>
    public static class IdentityExtention 
    {
        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<TestDBContext>().AddDefaultTokenProviders();
        }
    }
}