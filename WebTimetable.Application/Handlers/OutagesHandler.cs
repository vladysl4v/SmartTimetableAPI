using WebTimetable.Application.Entities;
using WebTimetable.Application.Handlers.Abstractions;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;

namespace WebTimetable.Application.Handlers
{
    public class OutagesHandler : IOutagesHandler
    {
        private readonly IDbRepository _dbRepository;
        public OutagesHandler(IDbRepository dbRepository)
        {
            _dbRepository = dbRepository;
        }

        public async Task ConfigureOutagesAsync(IEnumerable<Lesson> schedule, string outageGroup, string city)
        {
            foreach (var lesson in schedule)
            {
                var outages = await _dbRepository.FindAsync<OutageEntity>(city, outageGroup, lesson.Date.DayOfWeek);
                lesson.Outages =
                    outages?.Outages.Where(x => IsIntervalsIntersects(x.Start, x.End, lesson.Start, lesson.End))
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