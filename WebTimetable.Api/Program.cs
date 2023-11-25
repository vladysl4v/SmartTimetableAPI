using Microsoft.AspNetCore.HttpOverrides;
using Moesif.Middleware;
using WebTimetable.Api;
using WebTimetable.Api.Middleware;
using WebTimetable.Application;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(prefix: "CONFIG:");
var config = builder.Configuration;

builder.WebHost.ConfigureKestrel((_, options) => {
    options.AllowSynchronousIO = true;
});

if (builder.Environment.IsProduction())
{
    builder.Services.AddHttpsRedirection(options => { options.HttpsPort = 443; });
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });
}

builder.Services.AddDatabaseContext(config);
builder.Services.AddApplication();
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
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionsMiddleware>();
app.UseMiddleware<MoesifMiddleware>(new Dictionary<string, object> {
    {"ApplicationId", config.GetValue<string>("MoesifKey")!}
});

app.UseCors("PublicCORSPolicy");
app.UseOutputCache();

if (app.Environment.IsProduction())
{
    app.UseForwardedHeaders();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
