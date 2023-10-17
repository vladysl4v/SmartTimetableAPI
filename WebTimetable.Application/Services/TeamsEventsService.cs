using Microsoft.Graph;
using Microsoft.Graph.Models;

using WebTimetable.Application.Models;
using WebTimetable.Application.Services.Abstractions;

using EventModel = WebTimetable.Application.Models.Event;

namespace WebTimetable.Application.Services
{
    public class TeamsEventsService : IEventsService
    {
        private readonly GraphServiceClient _graphClient;
        private readonly int _utcOffset;
        public TeamsEventsService(GraphServiceClient graphServiceClient)
        {
            _graphClient = graphServiceClient;
            _utcOffset = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time").
                GetUtcOffset(DateTime.UtcNow).Hours;
        }

        public async Task ConfigureEvents(IEnumerable<Lesson> schedule)
        {
            if (!schedule.Any())
            {
                return;
            }
            var calendarData = await _graphClient.Me.Calendar.CalendarView.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.StartDateTime = schedule.First().Date.ToString("yyyy-MM-dd") + "T00:00:00";
                requestConfiguration.QueryParameters.EndDateTime = schedule.Last().Date.ToString("yyyy-MM-dd") + "T23:59:59";
                requestConfiguration.QueryParameters.Filter = "isCancelled eq false";
            });

            if (calendarData?.Value is null)
            {
                return;
            }

            var eventList = calendarData.Value.Select(anEvent => new EventModel
            {
                Title = anEvent.Subject,
                Link = anEvent.OnlineMeeting?.JoinUrl,
                Date = DateOnly.FromDateTime(anEvent.Start.ToDateTime()),
                StartTime = TimeOnly.FromDateTime(anEvent.Start.ToDateTime()).AddHours(_utcOffset),
                EndTime = TimeOnly.FromDateTime(anEvent.End.ToDateTime()).AddHours(_utcOffset)
            }).ToList();

            foreach (var lesson in schedule)
            {
                lesson.Events = eventList.FindAll(x => x.Date == lesson.Date &&
                                                       x.StartTime >= lesson.Start &&
                                                       x.StartTime < lesson.End &&
                                                       x.Link != null);
            }
        }
    }
}
