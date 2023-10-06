using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace WebTimetable.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        var apiInformation = new OpenApiInfo
        {
            Version = "v1",
            Title = "SmartTimetable API",
            Description = "Web API for schedule-based projects.",
            Contact = new OpenApiContact
            {
                Name = "Vladyslav Arkhypenkov",
                Email = "ArchipenkovV@krok.edu.ua",
                Url = new Uri("https://github.com/vladysl4v/"),
            },
            License = new OpenApiLicense
            {
                Name = "The GNU General Public License v3.0",
                Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.html#license-text")
            }
        };

        var securityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "Microsoft access token",
            Description = "Insert your microsoft access token",
            Name = "JWT Authentication",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        services.AddSwaggerGen(options =>
        {
            // Configure common information about API
            options.SwaggerDoc("v1", apiInformation);

            // Configure authorization button
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new List<string>() }
            });

            // Activate swagger controllers documentation
            options.CustomSchemaIds(type => type.ToString());

            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var xmlDocs = currentAssembly.GetReferencedAssemblies()
                .Union(new[] { currentAssembly.GetName() })
                .Select(a => Path.Combine(Path.GetDirectoryName(currentAssembly.Location)!, $"{a.Name}.xml"))
                .Where(File.Exists).ToArray();

            foreach (var doc in xmlDocs)
            {
                options.IncludeXmlComments(doc);
            }
        });

        return services;
    }
}