using WebTimetable.Api;
using WebTimetable.Api.Middleware;
using WebTimetable.Application;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddDatabaseContext(config.GetConnectionString("RenderPostgresConnection")!);
builder.Services.AddApplication(builder.Environment.IsDevelopment());

builder.Services.AddHttpClient();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureMicrosoftIdentityAuthentication(config);
builder.Services.ConfigureVersioning();
builder.Services.ConfigureCaching();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionsMiddleware>();
app.UseCors("PublicCORSPolicy");
app.UseOutputCache();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.Services.InitializeApplicationAsync();

app.Run();
