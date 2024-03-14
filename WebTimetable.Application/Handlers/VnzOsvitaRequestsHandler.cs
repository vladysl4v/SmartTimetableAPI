using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Models.Abstractions;

namespace WebTimetable.Application.Handlers;

public class VnzOsvitaRequestsHandler : IRequestHandler
{
    private readonly IHttpClientFactory _httpFactory;
    
    public VnzOsvitaRequestsHandler(IHttpClientFactory httpFactory)
    {
        _httpFactory = httpFactory;
    }
    
    public async Task<Dictionary<string, List<KeyValuePair<string, string>>>> GetStudentFilters(CancellationToken token)
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

    public async Task<List<KeyValuePair<string, string>>> GetStudentStudyGroups(string faculty, int course, int educForm, CancellationToken token)
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

    public async Task<List<KeyValuePair<string, string>>> GetTeacherFaculties(CancellationToken token)
    {
        var url = $"https://vnz.osvita.net/BetaSchedule.asmx/GetEmployeeFaculties?" +
                  $"aVuzID=11784";

        var httpClient = _httpFactory.CreateClient();
        try
        {
            string serializedData = await httpClient.GetStringAsync(url, token);
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<KeyValuePair<string, string>>>>(serializedData);
            return jsonData["d"];
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Faculties filters cannot be received.",
                "Error during loading/deserializing data from VNZ Osvita.");
        }
    }

    public async Task<List<KeyValuePair<string, string>>> GetTeacherChairs(string faculty, CancellationToken token)
    {
        var url = $"https://vnz.osvita.net/BetaSchedule.asmx/GetEmployeeChairs?" +
            $"aVuzID=11784&" +
            $"aFacultyID=\"{faculty}\"&" +
            $"aGiveStudyTimes=false";
        
        var httpClient = _httpFactory.CreateClient();
        try
        {
            string serializedData = await httpClient.GetStringAsync(url, token);
            var jsonData = JObject.Parse(serializedData)["d"]!;
            var chairsData = ((JArray)jsonData["chairs"]!).ToObject<List<KeyValuePair<string, string>>>()!;
            return chairsData;
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Chairs filters cannot be received.",
                "Error during loading/deserializing data from VNZ Osvita.");
        }
    }

    public async Task<List<KeyValuePair<string, string>>> GetTeacherEmployees(string faculty, string chair,
        CancellationToken token)
    {
        var url = $"https://vnz.osvita.net/BetaSchedule.asmx/GetEmployees?" +
                  $"aVuzID=11784&" +
                  $"aFacultyID=\"{faculty}\"&" +
                  $"aChairID=\"{chair}\"";
        
        var httpClient = _httpFactory.CreateClient();
        try
        {
            string serializedData = await httpClient.GetStringAsync(url, token);
            var jsonData = JsonConvert.DeserializeObject<Dictionary<string, List<KeyValuePair<string, string>>>>(serializedData);
            return jsonData["d"];
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Teachers filters cannot be received.",
                "Error during loading/deserializing data from VNZ Osvita.");
        }
    }
    
    public async Task<List<TeacherLesson>> GetTeacherSchedule(DateTime date, string teacherId, CancellationToken token)
    {
        var httpClient = _httpFactory.CreateClient();
        var formattedDate = date.ToString("dd.MM.yyyy");
        string url = "https://vnz.osvita.net/BetaSchedule.asmx/GetScheduleDataEmp?" +
                     "aVuzID=11784&" +
                     "aEmployeeID=\"" + teacherId + "\"&" +
                     "aStartDate=\"" + formattedDate + "\"&" +
                     "aEndDate=\"" + formattedDate + "\"&" +
                     "aStudyTypeID=null";

        Dictionary<string, List<TeacherLesson>>? response;
        try
        {
            string stringResponse = await httpClient.GetStringAsync(url, token);
            response = JsonConvert.DeserializeObject<Dictionary<string, List<TeacherLesson>>>(stringResponse, new TeacherLessonFactory());
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Schedule data cannot be received.",
                "Error during loading/deserializing data from the VNZ Osvita.");
        }

        if (response == null || !response.ContainsKey("d") || response["d"] == null)
        {
            throw new InternalServiceException("Schedule data cannot be received.",
                "Requested schedule cannot be properly deserialized.");
        }

        foreach (var lesson in response["d"])
        {
            lesson.Id = GenerateLessonIdentifier(lesson);
        }

        return response["d"];
    }
    
    public async Task<List<StudentLesson>> GetStudentSchedule(DateTime date, string groupId, CancellationToken token)
    {
        var httpClient = _httpFactory.CreateClient();
        var formattedDate = date.ToString("dd.MM.yyyy");
        string url = "https://vnz.osvita.net/BetaSchedule.asmx/GetScheduleDataX?" +
                     "aVuzID=11784&" +
                     "aStudyGroupID=\"" + groupId + "\"&" +
                     "aStartDate=\"" + formattedDate + "\"&" +
                     "aEndDate=\"" + formattedDate + "\"&" +
                     "aStudyTypeID=null";

        Dictionary<string, List<StudentLesson>>? response;
        try
        {
            string stringResponse = await httpClient.GetStringAsync(url, token);
            response = JsonConvert.DeserializeObject<Dictionary<string, List<StudentLesson>>>(stringResponse, new StudentLessonFactory());
        }
        catch (Exception ex) when (ex is not TaskCanceledException)
        {
            throw new InternalServiceException(ex, "Schedule data cannot be received.",
                "Error during loading/deserializing data from the VNZ Osvita.");
        }

        if (response == null || !response.ContainsKey("d") || response["d"] == null)
        {
            throw new InternalServiceException("Schedule data cannot be received.",
                "Requested schedule cannot be properly deserialized.");
        }

        foreach (var lesson in response["d"])
        {
            lesson.Id = GenerateLessonIdentifier(lesson);
        }

        return response["d"];
    }

    private Guid GenerateLessonIdentifier(ILesson lesson)
    {
        string compressedValue = lesson.Discipline +
                                 lesson.StudyType +
                                 lesson.Cabinet +
                                 lesson.Date.ToString("dd.MM.yyyy") +
                                 lesson.Start.ToString("HH:mm") +
                                 lesson.End.ToString("HH:mm");
        var hashedData = MD5.HashData(Encoding.Default.GetBytes(compressedValue));
        return new Guid(hashedData);
    }
}