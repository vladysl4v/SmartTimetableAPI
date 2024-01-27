using Microsoft.Graph;
using Microsoft.Graph.Models;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models.Abstractions;
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

        public async Task ConfigureEventsAsync(IEnumerable<ILesson> schedule, CancellationToken token)
        {
            var lessons = schedule.ToList();
            if (!lessons.Any())
            {
                return;
            }
            var calendarData = await _graphClient.Me.Calendar.CalendarView.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.StartDateTime = lessons.First().Date.ToString("yyyy-MM-dd") + "T00:00:00";
                requestConfiguration.QueryParameters.EndDateTime = lessons.Last().Date.ToString("yyyy-MM-dd") + "T23:59:59";
                requestConfiguration.QueryParameters.Filter = "isCancelled eq false";
            }, token);

            if (calendarData?.Value is null)
            {
                return;
            }

            var eventList = calendarData.Value.Select(anEvent => new Event
            {
                Title = anEvent.Subject,
                Link = anEvent.OnlineMeeting?.JoinUrl,
                Date = DateOnly.FromDateTime(anEvent.Start.ToDateTime()),
                StartTime = TimeOnly.FromDateTime(anEvent.Start.ToDateTime()).AddHours(_utcOffset),
                EndTime = TimeOnly.FromDateTime(anEvent.End.ToDateTime()).AddHours(_utcOffset)
            }).ToList();

            foreach (var lesson in lessons)
            {
                lesson.Events = eventList.FindAll(x => x.Date == lesson.Date &&
                                                       x.StartTime >= lesson.Start &&
                                                       x.StartTime < lesson.End &&
                                                       x.Link != null);
            }
        }
    }
}