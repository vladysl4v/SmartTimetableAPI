using Microsoft.AspNetCore.Mvc;

using WebTimetableApi.Handlers.Abstractions;
using WebTimetableApi.Models;
using WebTimetableApi.Schedules;
using WebTimetableApi.Schedules.Abstractions;
using WebTimetableApi.Schedules.Exceptions;


namespace WebTimetableApi.Controllers
{
    [Route("api/schedule")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IOutagesHandler _outagesHandler;

        private readonly IScheduleSource _scheduleSource;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleSource scheduleSource, IOutagesHandler outagesHandler)
        {
            _logger = logger;
            _scheduleSource = scheduleSource;
            _outagesHandler = outagesHandler;
        }

        [HttpGet]
        public async Task<List<Lesson>> Get(string date, bool provideOutages)
        {
            var selectedDate = DateTime.Parse(date);
            List<Lesson> lessons;
            try
            {
                lessons = await _scheduleSource.GetSchedule(selectedDate, "VZGSIZREGG8Y");
            }
            catch (ScheduleNotLoadedException ex)
            {
                _logger.LogError(ex, "Error retrieving schedule.");
                throw;
            }

            if (provideOutages)
            {
                foreach (var lesson in lessons)
                {
                    lesson.Outages = _outagesHandler.GetOutages(lesson.Start, lesson.End, lesson.Date.DayOfWeek);
                }
            }

            return lessons;
        }
    }
}
