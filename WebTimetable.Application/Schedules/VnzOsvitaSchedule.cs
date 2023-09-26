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
        public async Task<IEnumerable<Lesson>> GetSchedule(DateTime startDate, DateTime endDate, string groupId)
        {
            var httpClient = _httpFactory.CreateClient();
            string url =
                "https://vnz.osvita.net/BetaSchedule.asmx/GetScheduleDataX?" +
                "aVuzID=11784&" +
                "aStudyGroupID=\"" + groupId + "\"&" +
                "aStartDate=\"" + startDate.ToShortDateString() + "\"&" +
                "aEndDate=\"" + endDate.ToShortDateString() + "\"&" +
                "aStudyTypeID=null";

            Dictionary<string, IEnumerable<Lesson>>? response;
            try
            {
                string stringResponse = await httpClient.GetStringAsync(url);
                response = JsonConvert.DeserializeObject<Dictionary<string, IEnumerable<Lesson>>>(stringResponse, new LessonFactory());
            }
            catch (Exception ex)
            {
                throw new ScheduleNotLoadedException(ex, groupId, "Error during loading/deserializing.");
            }

            return response != null ? response["d"] : Enumerable.Empty<Lesson>();
        }
    }
}
