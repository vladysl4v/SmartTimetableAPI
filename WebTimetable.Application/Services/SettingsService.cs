using Newtonsoft.Json.Linq;

using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly IHttpClientFactory _httpFactory;
    public SettingsService(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
    }

    public async Task<Dictionary<string, Dictionary<string, string>>> GetFilters()
    {
        var url = "https://vnz.osvita.net/BetaSchedule.asmx/GetStudentScheduleFiltersData?&" +
                  "aVuzID=11784";

        var httpClient = _httpFactory.CreateClient();
        string serializedData = await httpClient.GetStringAsync(url);

        var jsonData = JObject.Parse(serializedData)["d"];

        return new Dictionary<string, Dictionary<string, string>>
        {
            {   "faculties", ((JArray)jsonData["faculties"]!)
                    .ToObject<List<KeyValuePair<string, string>>>()?
                        .ToDictionary(k => k.Key, v => v.Value)
            },
            {
                "educForms", ((JArray)jsonData["educForms"]!)
                    .ToObject<List<KeyValuePair<string, string>>>()?
                        .ToDictionary(k => k.Key, v => v.Value)
            },
            {
                "courses", ((JArray) jsonData["courses"]!)
                    .ToObject<List<KeyValuePair<string, string>>>()?
                        .ToDictionary(k => k.Key, v => v.Value)
            }
        };
    }

    public async Task<Dictionary<string, string>> GetStudyGroups(string faculty, int course, int educForm)
    {
        var url = $"https://vnz.osvita.net/BetaSchedule.asmx/GetStudyGroups?&" +
                  $"aVuzID=11784&" +
                  $"aFacultyID=\"{faculty}\"&" +
                  $"aEducationForm=\"{educForm}\"&" +
                  $"aCourse=\"{course}\"&" +
                  $"aGiveStudyTimes=false";

        var httpClient = _httpFactory.CreateClient();
        string serializedData = await httpClient.GetStringAsync(url);
        var jsonData = JObject.Parse(serializedData)["d"];

        return ((JArray)jsonData["studyGroups"]!)
            .ToObject<List<KeyValuePair<string, string>>>()?
            .ToDictionary(k => k.Key, v => v.Value);
    }
}