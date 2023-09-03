using Microsoft.Identity.Web;

using WebTimetableApi.DAL;
using WebTimetableApi.DAL.Entities;
using WebTimetableApi.DTOs;
using WebTimetableApi.Handlers;
using WebTimetableApi.Handlers.Abstractions;
using WebTimetableApi.Schedules;
using WebTimetableApi.Schedules.Abstractions;


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
builder.Services.AddAutoMapper(maps =>
{
    maps.CreateMap<UserEntity, UserDTO>();
});

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
    builder.Services.AddSingleton<IOutagesHandler, FakeOutagesHandler>();
}
else
{
    builder.Services.AddSingleton<IOutagesHandler, DtekOutagesHandler>();
}

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

await app.Services.GetRequiredService<IOutagesHandler>().InitializeOutages();

app.Run();
