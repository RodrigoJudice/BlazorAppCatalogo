using BlazorAppCatalogo.Server.Context;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppCatalogo.Server.Util.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddCustomServices
            (this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext(config.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection"));

            return services;
        }
        private static IServiceCollection AddDbContext
            (this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                           options.UseSqlite(connectionString));
            return services;
        }

    }

}
