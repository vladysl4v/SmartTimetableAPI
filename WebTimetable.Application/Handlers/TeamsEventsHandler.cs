using Microsoft.Graph;
using Microsoft.Graph.Models;
using WebTimetable.Application.Handlers.Abstractions;
using Event = WebTimetable.Application.Models.Event;

namespace WebTimetable.Application.Handlers
{
    public class TeamsEventsHandler : IEventsHandler
    {
        private readonly GraphServiceClient _graphClient;
        private readonly int _utcOffset;
        public TeamsEventsHandler(GraphServiceClient graphServiceClient)
        {
            _graphClient = graphServiceClient;
            _utcOffset = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time").
                GetUtcOffset(DateTime.UtcNow).Hours;
        }

        public async Task<List<Event>> GetEventsAsync(DateOnly date, TimeOnly start, TimeOnly end, CancellationToken token)
        {
            var requestStartTime = start.AddHours(-_utcOffset).ToString("HH:mm:ss");
            var requestEndTime = end.AddHours(-_utcOffset).ToString("HH:mm:ss");
            
            var calendarData = await _graphClient.Me.Calendar.CalendarView.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.StartDateTime = $"{date.ToString("yyyy-MM-dd")}T{requestStartTime}";
                requestConfiguration.QueryParameters.EndDateTime = $"{date.ToString("yyyy-MM-dd")}T{requestEndTime}";
                requestConfiguration.QueryParameters.Filter = "isCancelled eq false";
            }, token);

            if (calendarData?.Value is null)
            {
                return new List<Event>();
            }

            var eventList = calendarData.Value.Where(x => x.OnlineMeeting?.JoinUrl != null).Select(anEvent => new Event
            {
                Title = anEvent.Subject ?? "Не вказано",
                Link = anEvent.OnlineMeeting?.JoinUrl!,
                Date = DateOnly.FromDateTime(anEvent.Start.ToDateTime()),
                StartTime = TimeOnly.FromDateTime(anEvent.Start.ToDateTime()).AddHours(_utcOffset),
                EndTime = TimeOnly.FromDateTime(anEvent.End.ToDateTime()).AddHours(_utcOffset)
            }).ToList();

            return eventList;
        }
    }
}