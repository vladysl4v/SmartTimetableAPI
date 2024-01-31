using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Repositories.Abstractions;

namespace WebTimetable.Application.Services.Commands;

public partial class UpdateOutagesCommand : IJob
{
    private readonly ILogger<UpdateOutagesCommand> _logger;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IUsersRepository _usersRepository;
    private readonly IOutagesRepository _outagesRepository;
    
    public UpdateOutagesCommand(IOutagesRepository outagesRepository, IUsersRepository usersRepository, IHttpClientFactory httpFactory,
        ILogger<UpdateOutagesCommand> logger)
    {
        _logger = logger;
        _httpFactory = httpFactory;
        _usersRepository = usersRepository;
        _outagesRepository = outagesRepository;
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
        await _outagesRepository.AddOutagesAsync(outages, outageGroups, context.CancellationToken);
        _logger.LogInformation("Outages were successfully updated.");
        
        // TODO: Remove after successful testing
        await _usersRepository.LogOutageUpdateAsync(context.CancellationToken);
        
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
                    item => item.Value));
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