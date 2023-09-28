using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Models;
using WebTimetable.Application.Schedules.Abstractions;
using WebTimetable.Application.Schedules.Exceptions;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Controllers
{
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly IOutagesService _outagesService;
        private readonly IScheduleSource _scheduleSource;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleSource scheduleSource,
            IOutagesService outagesService)
        {
            _logger = logger;
            _dbRepository = dbRepository;
            _scheduleSource = scheduleSource;
            _outagesService = outagesService;
        }

        [ProducesResponseType(502)]
        [ProducesResponseType(typeof(AnonymousScheduleResponse), 200)]
        [HttpGet(ApiEndpoints.Schedule.GetSchedule)]
        public async Task<IActionResult> GetAnonymousSchedule([FromQuery] AnonymousScheduleRequest request)
        {
            List<Lesson> lessons;
            try
            {
                lessons = await _scheduleSource.GetSchedule(request.Start, request.End, request.StudyGroup);
            }
            catch (ScheduleNotLoadedException ex)
            {
                _logger.LogError(ex, "Error retrieving schedule.");
                return StatusCode(502);
            }

            if (request.OutageGroup != 0)
            {
                _outagesService.ConfigureOutages(lessons, request.OutageGroup);
        }

            var response = lessons.MapToAnonymousScheduleResponse();
            return Ok(response);
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
