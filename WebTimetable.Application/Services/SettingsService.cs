using Newtonsoft.Json.Linq;

using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Handlers.Outages;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IOutagesHandler _outagesHandler;
    public SettingsService(IHttpClientFactory httpFactory, IOutagesHandler outagesHandler)
    {
        _httpFactory = httpFactory;
        _outagesHandler = outagesHandler;
    }

    public List<string> GetOutageGroups()
    {
        return _outagesHandler.GetOutageGroups("Kyiv");
    }

    public async Task<Dictionary<string, Dictionary<string, string>>> GetFilters(CancellationToken token)
    {
        var url = "https://vnz.osvita.net/BetaSchedule.asmx/GetStudentScheduleFiltersData?&" +
                  "aVuzID=11784";

        var httpClient = _httpFactory.CreateClient();

        try
        {
            string serializedData = await httpClient.GetStringAsync(url, token);
            var jsonData = JObject.Parse(serializedData)["d"];

            var outputFilters = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "faculties", ((JArray)jsonData["faculties"]!)
                                    .ToObject<List<KeyValuePair<string, string>>>()!
                                    .ToDictionary(k => k.Key, v => v.Value)
                },
                {
                    "educForms", ((JArray)jsonData["educForms"]!)
                                    .ToObject<List<KeyValuePair<string, string>>>()!
                                    .ToDictionary(k => k.Key, v => v.Value)
                },
                {
                    "courses", ((JArray)jsonData["courses"]!)
                                    .ToObject<List<KeyValuePair<string, string>>>()!
                                    .ToDictionary(k => k.Key, v => v.Value)
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

    public async Task<Dictionary<string, string>> GetStudyGroups(string faculty, int course, int educForm, CancellationToken token)
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
            var jsonData = JObject.Parse(serializedData)["d"];

            return ((JArray)jsonData["studyGroups"]!)
                                .ToObject<List<KeyValuePair<string, string>>>()!
                                .ToDictionary(k => k.Key, v => v.Value);
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Study groups filters cannot be received.",
                "Error during loading/deserializing data from VNZ Osvita.");
        }
    }
}