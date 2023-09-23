using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WebTimetable.Application.Repositories;


namespace WebTimetable.Application
{
    public static class DatabaseServicesExtensions
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            services.AddScoped<IDbRepository, DbRepository>();

            return services;
        }
    }
}
