using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;

namespace WebTimetable.Application.Services.Commands;

public partial class UpdateOutagesCommand : IJob
{
    private readonly ILogger<UpdateOutagesCommand> _logger;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IRepository<UserEntity> _users;
    private readonly IRepository<OutageEntity> _outages;
    
    public UpdateOutagesCommand(IRepository<OutageEntity> outages, IRepository<UserEntity> users, IHttpClientFactory httpFactory,
        ILogger<UpdateOutagesCommand> logger)
    {
        _logger = logger;
        _httpFactory = httpFactory;
        _users = users;
        _outages = outages;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var source = "https://www.dtek-kem.com.ua/ua/shutdowns";
        var httpClient = _httpFactory.CreateClient();
        
        var response = await httpClient.GetStringAsync(source);
        string serializedData, serializedGroups;
        try
        {
            serializedData = OutagesRegex().Match(response).Value[7..^1];
            serializedGroups = OutageGroupsRegex().Match(response).Value[12..];
        }
        catch
        {
            _logger.LogCritical("Outages or outage groups are empty.");
            _logger.LogCritical("Response by DTEK:\n" + response);
            return;
        }

        var outageGroups = DeserializeGroups(serializedGroups);
        var outages = DeserializeObject(serializedData);
        
        if (!outages.Any() || !outageGroups.Any())
        {
            _logger.LogCritical("Outages or outage groups are empty.");
            return;
        }
        
        var isSuccessful = await FillDatabase(outageGroups, outages, context.CancellationToken);
        if (isSuccessful)
        {
            _logger.LogInformation("Outages were successfully updated.");
            // TODO: Remove after successful testing
            await _users.AddAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                FullName = "Outages updated",
                Group = "Admin",
                IsRestricted = false
            }, context.CancellationToken);
            await _users.SaveChangesAsync(context.CancellationToken);
        }
    }
    
    private async Task<bool> FillDatabase(Dictionary<string, string> outageGroups,
        Dictionary<int, Dictionary<DayOfWeek, List<Outage>>> outages, CancellationToken token)
    {
        try
        {
            _outages.RemoveRange(_outages.GetAll());
            foreach (var group in outages)
            {
                foreach (var dayOfWeek in group.Value)
                {
                    await _outages.AddAsync(new OutageEntity
                    {
                        City = "Kyiv",
                        Group = outageGroups[group.Key.ToString()],
                        DayOfWeek = dayOfWeek.Key,
                        Outages = dayOfWeek.Value
                    }, token);
                }
            }

            await _outages.SaveChangesAsync(token);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return false;
        }

        return true;
    }
    
    private Dictionary<string, string> DeserializeGroups(string serializedGroups)
    {
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(serializedGroups)!;
    }

    private Dictionary<int, Dictionary<DayOfWeek, List<Outage>>> DeserializeObject(string serializedData)
    {
        var allGroups =
            JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<Outage>>>>(serializedData,
                new OutageFactory());

        return allGroups.ToDictionary(group => int.Parse(group.Key),
                group => group.Value.ToDictionary(item => ConvertToDayOfWeek(item.Key),
                    item => item.Value.Where(x => x.IsDefinite is not null).ToList()));
    }

    private DayOfWeek ConvertToDayOfWeek(string value)
    {
        var integer = int.Parse(value);
        return integer == 7 ? DayOfWeek.Sunday : (DayOfWeek)integer;
    }

    [GeneratedRegex("\"data\":{.*")]
    private static partial Regex OutagesRegex();
    
    [GeneratedRegex("\"sch_names\":{.*?}")]
    private static partial Regex OutageGroupsRegex();
}