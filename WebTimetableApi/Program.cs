using WebTimetableApi.Handlers.Abstractions;
using WebTimetableApi.Handlers;
using WebTimetableApi.Schedules;
using WebTimetableApi.Schedules.Abstractions;


var builder = WebApplication.CreateBuilder(args);

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("PublicCORSPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.GetRequiredService<IOutagesHandler>().InitializeOutages();

app.Run();
