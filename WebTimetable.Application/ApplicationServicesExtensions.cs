using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WebTimetable.Application.Repositories;
using WebTimetable.Application.Schedules;
using WebTimetable.Application.Schedules.Abstractions;
using WebTimetable.Application.Services;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application
{
    public static class ApplicationServicesExtensions
    {
        public static async Task InitializeApplicationAsync(this IServiceProvider services)
        {
            await services.GetRequiredService<IOutagesService>().InitializeOutages();
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, bool isDevelopment)
        {
            if (isDevelopment)
            {
                services.AddSingleton<IOutagesService, FakeOutagesService>();
            }
            else
            {
                services.AddSingleton<IOutagesService, DtekOutagesService>();
            }

            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IScheduleSource, VnzOsvitaSchedule>();

            return services;
        }

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
