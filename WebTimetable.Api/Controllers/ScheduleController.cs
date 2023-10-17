using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Schedules.Abstractions;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IOutagesService _outagesService;
        private readonly IScheduleSource _scheduleSource;
        private readonly IEventsService _eventsService;
        private readonly INotesService _notesService;
        private readonly IUsersService _usersService;

        public ScheduleController(IScheduleSource scheduleSource, IOutagesService outagesService,
            INotesService notesService, IUsersService usersService, IEventsService eventsService)
        {
            _usersService = usersService;
            _notesService = notesService;
            _eventsService = eventsService;
            _scheduleSource = scheduleSource;
            _outagesService = outagesService;
        }

        [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "ScheduleCache")]
        [HttpGet(ApiEndpoints.Schedule.GetAnonymousSchedule)]
        public async Task<IActionResult> GetAnonymousSchedule([FromQuery] AnonymousScheduleRequest request, CancellationToken token)
        {
            if (!DateTime.TryParse(request.StartDate, out var start) ||
                !DateTime.TryParse(request.EndDate, out var end))
            {
                return BadRequest(
                    new ValidationProblemDetails(new Dictionary<string, string[]>
                        {
                            { "startDate", new[] { "The given value cannot be converted to DateTime." } },
                            { "endDate", new[] { "The given value cannot be converted to DateTime." } }
                        }));
            }

            var lessons = await _scheduleSource.GetSchedule(start, end, request.StudyGroup, token);
            
            if (request.OutageGroup != 0)
            {
                _outagesService.ConfigureOutages(lessons, request.OutageGroup);
            }

            var response = lessons.MapToScheduleResponse();
            return Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpGet(ApiEndpoints.Schedule.GetPersonalSchedule)]
        public async Task<IActionResult> GetPersonalSchedule([FromQuery] PersonalScheduleRequest request, CancellationToken token)
        {
            if (!DateTime.TryParse(request.StartDate, out var start) ||
                !DateTime.TryParse(request.EndDate, out var end))
            {
                return BadRequest(
                    new ValidationProblemDetails(new Dictionary<string, string[]>
                    {
                        { "startDate", new[] { "The given value cannot be converted to DateTime." } },
                        { "endDate", new[] { "The given value cannot be converted to DateTime." } }
                    }));
            }
            var lessons = await _scheduleSource.GetSchedule(start, end, request.StudyGroup, token);

            if (request.OutageGroup != 0)
            {
                _outagesService.ConfigureOutages(lessons, request.OutageGroup);
            }

            var user = await _usersService.GetUser(token);

            if (user is null)
            {
                return Unauthorized("User department cannot be accessed by server.");
            }

            await _eventsService.ConfigureEvents(lessons);
            _notesService.ConfigureNotes(lessons, user.Group);

            var response = lessons.MapToScheduleResponse(user.Id);
            return Ok(response);
        }
    }
}
