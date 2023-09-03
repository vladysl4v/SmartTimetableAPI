using Microsoft.AspNetCore.Mvc;

using WebTimetableApi.Handlers.Abstractions;
using WebTimetableApi.Models;
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
        public async Task<IActionResult> GetSchedule(string date, bool provideOutages)
        {
            if (!DateTime.TryParse(date, out var selectedDate))
            {
                return BadRequest();
            }
            List<Lesson> lessons;
            try
            {
                lessons = await _scheduleSource.GetSchedule(selectedDate, "VZGSIZREGG8Y");
            }
            catch (ScheduleNotLoadedException ex)
            {
                _logger.LogError(ex, "Error retrieving schedule.");
                return StatusCode(502);
            }

            if (provideOutages)
            {
                foreach (var lesson in lessons)
                {
                    lesson.Outages = _outagesHandler.GetOutages(lesson.Start, lesson.End, lesson.Date.DayOfWeek);
                }
            }

            return Ok(lessons);
        }
    }
}
