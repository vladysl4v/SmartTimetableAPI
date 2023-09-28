using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

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

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
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
