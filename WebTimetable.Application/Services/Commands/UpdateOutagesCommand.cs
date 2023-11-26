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
    private readonly IDbRepository _dbRepository;
    
    public UpdateOutagesCommand(IDbRepository dbRepository, IHttpClientFactory httpFactory,
        ILogger<UpdateOutagesCommand> logger)
    {
        _logger = logger;
        _httpFactory = httpFactory;
        _dbRepository = dbRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var source = "https://www.dtek-kem.com.ua/ua/shutdowns";
        var httpClient = _httpFactory.CreateClient();
        
        var request = await httpClient.GetStringAsync(source);
        _logger.LogInformation(request);
        string serializedData, serializedGroups;
        try
        {
            serializedData = OutagesRegex().Match(request).Value[7..^1];
            serializedGroups = OutageGroupsRegex().Match(request).Value[12..];
        }
        catch
        {
            _logger.LogCritical("Outages or outage groups are empty.");
            return;
        }

        var outageGroups = DeserializeGroups(serializedGroups);
        var outages = DeserializeObject(serializedData);
        
        if (!outages.Any() || !outageGroups.Any())
        {
            _logger.LogCritical("Outages or outage groups are empty.");
            return;
        }
        
        var isSuccessful = await FillDatabase(outageGroups, outages);
        if (isSuccessful)
        {
            _logger.LogInformation("Outages were successfully updated.");
        }
    }
    
    private async Task<bool> FillDatabase(Dictionary<string, string> outageGroups,
        Dictionary<int, Dictionary<DayOfWeek, List<Outage>>> outages)
    {
        try
        {
            await _dbRepository.GetAll<OutageEntity>().ExecuteDeleteAsync();
            foreach (var group in outages)
            {
                foreach (var dayOfWeek in group.Value)
                {
                    await _dbRepository.Add(new OutageEntity
                    {
                        City = "Kyiv",
                        Group = outageGroups[$"{group.Key}"],
                        DayOfWeek = dayOfWeek.Key,
                        Outages = dayOfWeek.Value
                    });
                }
            }

            await _dbRepository.SaveChangesAsync(CancellationToken.None);
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