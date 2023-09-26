using Microsoft.Identity.Web;

using WebTimetable.Application;
using WebTimetable.Application.Services;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Application.Schedules;
using WebTimetable.Application.Schedules.Abstractions;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDatabaseContext(config.GetConnectionString("PostgresConnection")!);

builder.Services.AddMicrosoftIdentityWebApiAuthentication(config)
        .EnableTokenAcquisitionToCallDownstreamApi()
        .AddMicrosoftGraph(config.GetSection("GraphClient"))
        .AddInMemoryTokenCaches();

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "PublicCORSPolicy", policy =>
        {
            policy.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<IOutagesService, FakeOutagesService>();
}
else
{
    builder.Services.AddSingleton<IOutagesService, DtekOutagesService>();
}

builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IScheduleSource, VnzOsvitaSchedule>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PublicCORSPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.Services.GetRequiredService<IOutagesService>().InitializeOutages();

app.Run();
