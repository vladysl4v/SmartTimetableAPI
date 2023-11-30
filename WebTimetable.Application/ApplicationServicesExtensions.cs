using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebTimetable.Application.Handlers;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Application.Services.Commands;


namespace WebTimetable.Application
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<INotesHandler, NotesHandler>();
            services.AddScoped<IEventsHandler, TeamsEventsHandler>();
            services.AddScoped<IScheduleHandler, VnzOsvitaScheduleHandler>();
            services.AddScoped<IOutagesHandler, OutagesHandler>();
            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<UpdateOutagesCommand>();

            return services;
        }

        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration config,
            string connectionStringName)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString(connectionStringName));
            });
            services.AddScoped<IDbRepository, DbRepository>();

            return services;
        }
    }
}
