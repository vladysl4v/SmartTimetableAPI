using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Models;
using WebTimetable.Application.Schedules.Abstractions;
using WebTimetable.Application.Schedules.Exceptions;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Api.Controllers
{
    [Authorize]
    [RequiredScope("access_as_user")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IOutagesService _outagesHandler;
        private readonly IDbRepository _dbRepository;

        private readonly IScheduleSource _scheduleSource;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleSource scheduleSource,
            IOutagesService outagesHandler, IDbRepository dbRepository)
        {
            _logger = logger;
            _dbRepository = dbRepository;
            _scheduleSource = scheduleSource;
            _outagesHandler = outagesHandler;
        }

        [AllowAnonymous]
        [HttpGet(ApiEndpoints.Schedule.Get)]
        public async Task<IActionResult> GetAnonymousSchedule([FromRoute]string date)
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

            return Ok(lessons);
        }

        
        [HttpGet(ApiEndpoints.Schedule.GetPersonal)]
        public async Task<IActionResult> GetAuthorizedSchedule([FromRoute] string date)
        {
            if (!DateTime.TryParse(date, out var selectedDate))
            {
                return BadRequest();
            }

            var userEntity = _dbRepository.Get<UserEntity>(x => x.Id == User.GetObjectId()).SingleOrDefault();

            if (userEntity == null)
            {
                return Unauthorized();
            }

            List<Lesson> lessons;
            try
            {
                lessons = await _scheduleSource.GetSchedule(selectedDate, userEntity.StudyGroupId);
            }
            catch (ScheduleNotLoadedException ex)
            {
                _logger.LogError(ex, "Error retrieving schedule.");
                return StatusCode(502);
            }
            
            if (userEntity.OutagesGroup != 0)
            {
                foreach (var lesson in lessons)
                {
                    lesson.Outages = _outagesHandler.GetOutages(lesson.Start, lesson.End, lesson.Date.DayOfWeek, userEntity.OutagesGroup);
                }
            }

            return Ok(lessons);
        }
    }
}
