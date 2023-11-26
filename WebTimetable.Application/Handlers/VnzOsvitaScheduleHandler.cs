using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Exceptions;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;

namespace WebTimetable.Application.Handlers
{
    public class VnzOsvitaScheduleHandler : IScheduleHandler
    {
        private readonly IHttpClientFactory _httpFactory;
        public VnzOsvitaScheduleHandler(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        public async Task<List<Lesson>> GetSchedule(DateTime date, string groupId, CancellationToken token)
        {
            var httpClient = _httpFactory.CreateClient();
            var formattedDate = date.ToString("dd.MM.yyyy");
            string url = "https://vnz.osvita.net/BetaSchedule.asmx/GetScheduleDataX?" +
                         "aVuzID=11784&" +
                         "aStudyGroupID=\"" + groupId + "\"&" +
                         "aStartDate=\"" + formattedDate + "\"&" +
                         "aEndDate=\"" + formattedDate + "\"&" +
                         "aStudyTypeID=null";

            Dictionary<string, List<Lesson>>? response;
            try
            {
                string stringResponse = await httpClient.GetStringAsync(url, token);
                response = JsonConvert.DeserializeObject<Dictionary<string, List<Lesson>>>(stringResponse, new LessonFactory());
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
                lesson.Id = GenerateLessonIdentifier(lesson, groupId);
            }

            return response["d"];
        }

        private Guid GenerateLessonIdentifier(Lesson lesson, string groupId)
        {
            string compressedValue = lesson.Discipline + lesson.StudyType + groupId + lesson.Date.ToString("dd.MM.yyyy") +
                                        lesson.Start.ToString("HH:mm");
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(compressedValue));
            return new Guid(data);
        }
    }
}
