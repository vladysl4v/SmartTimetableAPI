using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Models.Abstractions;
using WebTimetable.Application.Repositories.Abstractions;

namespace WebTimetable.Application.Handlers
{
    public class OutagesHandler : IOutagesHandler
    {
        private readonly IOutagesRepository _outagesRepository;
        public OutagesHandler(IOutagesRepository outagesRepository)
        {
            _outagesRepository = outagesRepository;
        }

        public async Task ConfigureOutagesAsync(IEnumerable<ILesson> schedule, string outageGroup, CancellationToken token)
        {
            foreach (var lesson in schedule)
            {
                var outages = await _outagesRepository.GetOutagesByDayOfWeekAsync(lesson.Date.DayOfWeek, outageGroup, token);
                lesson.Outages =
                    outages?.Where(x => IsIntervalsIntersects(x.Start, x.End, lesson.Start, lesson.End))
                        .ToList() ?? new List<Outage>();
            }
        }
        
        private bool IsIntervalsIntersects(TimeOnly start1, TimeOnly end1, TimeOnly start2, TimeOnly end2)
        {
            var isStartIntersects = start2 <= start1 && start1 < end2;
            var isEndIntersects = start2 < end1 && end1 <= end2;
            var isInside = start1 <= start2 && end1 >= end2;
            return isStartIntersects || isEndIntersects || isInside;
        }
    }
}