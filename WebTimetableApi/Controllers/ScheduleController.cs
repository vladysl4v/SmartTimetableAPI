using Microsoft.AspNetCore.Mvc;

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

        private readonly IScheduleSource _scheduleSource;

        public ScheduleController(ILogger<ScheduleController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;

            _scheduleSource = new VnzOsvitaSchedule(httpClientFactory);
        }

        [HttpGet]
        public async Task<List<Lesson>> Get(string date)
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

            return lessons;
        }
    }
}
