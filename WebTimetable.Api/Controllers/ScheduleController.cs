using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IUsersService _usersService;

        public ScheduleController(IScheduleService scheduleService, IUsersService usersService)
        {
            _scheduleService = scheduleService;
            _usersService = usersService;
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

            var lessons = await _scheduleService.GetGuestSchedule(start, end, request.StudyGroup, request.OutageGroup, token);

            var response = lessons.MapToScheduleResponse();
            return Ok(response);
        }

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
            var user = await _usersService.GetUser(token);
            var lessons = await _scheduleService.GetPersonalSchedule(start, end, request.StudyGroup, request.OutageGroup, user, token);
            if (lessons is null)
            {
                return Forbid();
            }

            var response = lessons.MapToScheduleResponse();
            return Ok(response);
        }
    }
}
