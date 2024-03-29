﻿using System.Text.RegularExpressions;
using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Identity.Web;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.OpenApi.Models;
using Moesif.Middleware;
using Quartz;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using WebTimetable.Api.Validators;
using WebTimetable.Application.Services.Commands;
using WebTimetable.Contracts.Requests;

namespace WebTimetable.Api;

public static class ServiceCollectionExtensions
{
    public static ConfigurationManager ConfigureEnvironmentVariables(this ConfigurationManager configuration)
    {
        configuration.AddEnvironmentVariables(prefix: "CONFIG:");
        
        // Parse provided by Heroku environment variable of database
        var herokuDatabaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (herokuDatabaseUrl == null)
        {
            return configuration;
        }
        var m = Regex.Match(Environment.GetEnvironmentVariable("DATABASE_URL")!, @"postgres://(.*):(.*)@(.*):(.*)/(.*)");
        configuration.GetSection("ConnectionStrings:Database").Value =
            $"Server={m.Groups[3]};Port={m.Groups[4]};User Id={m.Groups[1]};Password={m.Groups[2]};Database={m.Groups[5]};";
        
        return configuration;
    }
    
    public static WebApplication UseMoesif(this WebApplication app, string key)
    {
        
        app.UseMiddleware<MoesifMiddleware>(new Dictionary<string, object> {
            {"ApplicationId", app.Configuration.GetValue<string>(key)!}
        });
        return app;
    }

    public static IServiceCollection ConfigureCronJob(this IServiceCollection services, string cronExpression)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey(nameof(UpdateOutagesCommand));
            q.AddJob<UpdateOutagesCommand>(x => x.WithIdentity(jobKey));
            q.AddTrigger(x => x
                .ForJob(jobKey)
                .WithIdentity(nameof(UpdateOutagesCommand) + "Trigger")
                .WithCronSchedule(cronExpression));
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        return services;
    }
    public static IServiceCollection ConfigureCors(this IServiceCollection services, string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(name: policyName, policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
            });
        });
        return services;
    }

    public static IServiceCollection ConfigureValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddNoteValidator>();
        services.AddFluentValidationAutoValidation();
        return services;
    }
    
    public static IServiceCollection ConfigureMicrosoftIdentityAuthentication(this IServiceCollection services,
        IConfiguration config, string azureSection, string graphSection)
    {
        services.AddMicrosoftIdentityWebApiAuthentication(config, configSectionName: azureSection)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(config.GetSection(graphSection))
            .AddInMemoryTokenCaches();

        return services;
    }

    public static IServiceCollection ConfigureCaching(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddBasePolicy(policy => policy.Cache());

            options.AddPolicy("FiltersCache", policy => policy.Cache()
                .Expire(TimeSpan.FromMinutes(60))
                .SetVaryByRouteValue(new[] { "city" })
                .SetVaryByQueryFromProperties(typeof(StudyGroupsRequest), typeof(EmployeesRequest), typeof(ChairsRequest)));

            options.AddPolicy("ScheduleCache", policy => policy.Cache()
                .Expire(TimeSpan.FromMinutes(5))
                .SetVaryByQuery(new[] { "outageGroup" })
                .SetVaryByRouteFromProperties(typeof(ScheduleRequest)));
        });
        return services;
    }

    public static IServiceCollection ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1.0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
        }).AddMvc();
        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        var apiInformation = new OpenApiInfo
        {
            Version = "v1",
            Title = "SmartTimetable API",
            Description = "Web API for schedule-based projects.",
            Contact = new OpenApiContact
            {
                Name = "Vladyslav Arkhypenkov",
                Email = "arkhypenkov.corp@gmail.com",
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
            options.DescribeAllParametersInCamelCase();
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

    private static void SetVaryByQueryFromProperties(this OutputCachePolicyBuilder builder, params Type[] types)
    {
        var properties = new List<string>();
        foreach (var type in types)
        {
            properties.AddRange(type.GetProperties().Select(x => x.Name.ToFirstCharacterLowerCase()!));
        }
        builder.SetVaryByQuery(properties.Distinct().ToArray());
    }
    
    private static void SetVaryByRouteFromProperties(this OutputCachePolicyBuilder builder, params Type[] types)
    {
        var properties = new List<string>();
        foreach (var type in types)
        {
            properties.AddRange(type.GetProperties().Select(x => x.Name.ToFirstCharacterLowerCase()!));
        }
        builder.SetVaryByRouteValue(properties.Distinct().ToArray());
    }
}