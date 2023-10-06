using System.Security.Cryptography;
using System.Text;

using Newtonsoft.Json;

using WebTimetable.Application.Deserializers;
using WebTimetable.Application.Models;
using WebTimetable.Application.Schedules.Abstractions;
using WebTimetable.Application.Schedules.Exceptions;


namespace WebTimetable.Application.Schedules
{
    public class VnzOsvitaSchedule : IScheduleSource
    {
        private readonly IHttpClientFactory _httpFactory;
        public VnzOsvitaSchedule(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        public async Task<List<Lesson>> GetSchedule(DateTime startDate, DateTime endDate, string groupId)
        {
            var httpClient = _httpFactory.CreateClient();
            string url =
                "https://vnz.osvita.net/BetaSchedule.asmx/GetScheduleDataX?" +
                "aVuzID=11784&" +
                "aStudyGroupID=\"" + groupId + "\"&" +
                "aStartDate=\"" + startDate.ToShortDateString() + "\"&" +
                "aEndDate=\"" + endDate.ToShortDateString() + "\"&" +
                "aStudyTypeID=null";

            Dictionary<string, List<Lesson>>? response;
            try
            {
                string stringResponse = await httpClient.GetStringAsync(url);
                response = JsonConvert.DeserializeObject<Dictionary<string, List<Lesson>>>(stringResponse, new LessonFactory());
            }
            catch (Exception ex)
            {
                throw new ScheduleNotLoadedException(ex, groupId, "Error during loading/deserializing.");
            }

            if (response == null)
            {
                return new List<Lesson>(0);
            }

            foreach (var lesson in response["d"])
            {
                lesson.Id = GenerateLessonIdentifier(lesson, groupId);
            }
            return response != null ? response["d"] : new List<Lesson>(0);
        }

        private Guid GenerateLessonIdentifier(Lesson lesson, string groupId)
        {
            string compressedValue = lesson.Discipline + lesson.StudyType + groupId + lesson.Date.ToShortDateString() +
                                        lesson.Start.ToShortTimeString();
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(compressedValue));
            return new Guid(data);
        }
    }
}
