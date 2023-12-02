using Newtonsoft.Json.Linq;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IRepository<OutageEntity> _outages;
    public SettingsService(IHttpClientFactory httpFactory, IRepository<OutageEntity> outages)
    {
        _httpFactory = httpFactory;
        _outages = outages;
    }

    public async Task<Dictionary<string, List<KeyValuePair<string, string>>>> GetFilters(CancellationToken token)
    {
        var url = "https://vnz.osvita.net/BetaSchedule.asmx/GetStudentScheduleFiltersData?&" +
                  "aVuzID=11784";
        var httpClient = _httpFactory.CreateClient();
        try
        {
            string serializedData = await httpClient.GetStringAsync(url, token);
            var jsonData = JObject.Parse(serializedData)["d"]!;
            var outputFilters = new Dictionary<string, List<KeyValuePair<string, string>>>
            {
                {
                    "faculties", ((JArray)jsonData["faculties"]!).ToObject<List<KeyValuePair<string, string>>>()!
                },
                {
                    "educForms", ((JArray)jsonData["educForms"]!).ToObject<List<KeyValuePair<string, string>>>()!
                },
                {
                    "courses", ((JArray)jsonData["courses"]!).ToObject<List<KeyValuePair<string, string>>>()!
                },
                {
                    "outageGroups", GetOutageGroups()
                }
            };

            return outputFilters;
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Settings filters cannot be received.",
                "Error during loading/deserializing data from VNZ Osvita.");
        }
    }

    public async Task<List<KeyValuePair<string, string>>> GetStudyGroups(string faculty, int course, int educForm, CancellationToken token)
    {
        var url = $"https://vnz.osvita.net/BetaSchedule.asmx/GetStudyGroups?&" +
                  $"aVuzID=11784&" +
                  $"aFacultyID=\"{faculty}\"&" +
                  $"aEducationForm=\"{educForm}\"&" +
                  $"aCourse=\"{course}\"&" +
                  $"aGiveStudyTimes=false";

        var httpClient = _httpFactory.CreateClient();
        try
        {
            string serializedData = await httpClient.GetStringAsync(url, token);
            var jsonData = JObject.Parse(serializedData)["d"]!;

            return ((JArray)jsonData["studyGroups"]!).ToObject<List<KeyValuePair<string, string>>>()!;
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Study groups filters cannot be received.",
                "Error during loading/deserializing data from VNZ Osvita.");
        }
    }
    
    private List<KeyValuePair<string, string>> GetOutageGroups()
    {
        return _outages.Where(x => x.City == "Kyiv").Select(y => y.Group)
            .Distinct().ToDictionary(key => key, value => value).ToList();
    }
}