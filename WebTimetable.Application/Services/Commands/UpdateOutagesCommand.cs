using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;

namespace WebTimetable.Application.Services.Commands;

public partial class UpdateOutagesCommand
{
    private readonly ILogger<UpdateOutagesCommand> _logger;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IDbRepository _dbRepository;
    
    private DateTime _lastUpdate = DateTime.MinValue;
    
    public UpdateOutagesCommand(IDbRepository dbRepository, IHttpClientFactory httpFactory,
        ILogger<UpdateOutagesCommand> logger)
    {
        _logger = logger;
        _httpFactory = httpFactory;
        _dbRepository = dbRepository;
    }

    public async Task<bool> ExecuteAsync()
    {
        if (!(DateTime.Now - _lastUpdate >= TimeSpan.FromHours(4)))
        {
            _logger.LogInformation("Outages are up to date.");
            return false;
        }
        
        var source = "https://www.dtek-kem.com.ua/ua/shutdowns";
        var httpClient = _httpFactory.CreateClient();
        var request = await httpClient.GetStringAsync(source);
        
        var serializedData = OutagesRegex().Match(request).Value[7..^1];
        var serializedGroups = OutageGroupsRegex().Match(request).Value[12..];

        var outageGroups = DeserializeGroups(serializedGroups);
        var outages = DeserializeObject(serializedData);
        
        if (!outages.Any() || !outageGroups.Any())
        {
            _logger.LogCritical("Outages or outage groups are empty.");
            return false;
        }
        
        var isSuccessful = await FillDatabase(outageGroups, outages);
        if (isSuccessful)
        {
            _lastUpdate = DateTime.Now;
        }
        return isSuccessful;
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