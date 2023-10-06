using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

using WebTimetable.Api;
using WebTimetable.Application;


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

builder.Services.AddApplication(builder.Environment.IsDevelopment());

builder.Services.AddSwaggerConfiguration();

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

await app.Services.InitializeApplicationAsync();

app.Run();
