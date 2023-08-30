using Newtonsoft.Json;

using WebTimetableApi.Deserializers;
using WebTimetableApi.Models;
using WebTimetableApi.Schedules.Abstractions;
using WebTimetableApi.Schedules.Exceptions;


namespace WebTimetableApi.Schedules
{
    public class VnzOsvitaSchedule : IScheduleSource
    {
        private readonly IHttpClientFactory _httpFactory;
        public VnzOsvitaSchedule(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }
        public async Task<List<Lesson>> GetSchedule(DateTime date, string groupId)
        {
            var httpClient = _httpFactory.CreateClient();
            string url =
                "https://vnz.osvita.net/BetaSchedule.asmx/GetScheduleDataX?" +
                "aVuzID=99999&" +
                "aStudyGroupID=\"" + groupId + "\"&" +
                "aStartDate=\"" + date.ToShortDateString() + "\"&" +
                "aEndDate=\"" + date.ToShortDateString() + "\"&" +
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

            return response != null ? response["d"] : new List<Lesson>();
        }
    }
}
