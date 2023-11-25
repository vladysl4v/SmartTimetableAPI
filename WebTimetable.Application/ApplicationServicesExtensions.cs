using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebTimetable.Application.Handlers.Events;
using WebTimetable.Application.Handlers.Notes;
using WebTimetable.Application.Handlers.Outages;
using WebTimetable.Application.Handlers.Schedule;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application
{
    public static class ApplicationServicesExtensions
    {
        public static async Task InitializeApplicationAsync(this IServiceProvider services)
        {
            await services.GetRequiredService<IOutagesHandler>().InitializeOutages();
        }

        public static IServiceCollection AddApplication(this IServiceCollection services, bool isDevelopment)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<INotesHandler, NotesHandler>();
            services.AddScoped<IEventsHandler, TeamsEventsHandler>();
            services.AddScoped<IScheduleHandler, VnzOsvitaScheduleHandler>();

            if (isDevelopment)
            {
                services.AddSingleton<IOutagesHandler, FakeOutagesHandler>();
            }
            else
            {
                //services.AddSingleton<IOutagesHandler, DtekOutagesHandler>();
                services.AddSingleton<IOutagesHandler, FakeOutagesHandler>();
            }

            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<UpdateOutagesCommand>();

            return services;
        }

        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration config)
        {
            var connStringKey = config.GetSection("ASPNETCORE_ENVIRONMENT").Value == "Production" ? "DatabaseProd" : "Database";
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString(connStringKey));
            });
            services.AddScoped<IDbRepository, DbRepository>();

            return services;
        }
    }
}
