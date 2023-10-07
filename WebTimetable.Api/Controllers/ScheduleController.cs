using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Models;
using WebTimetable.Application.Schedules.Abstractions;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;


namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly GraphServiceClient _graphClient;
        private readonly IOutagesService _outagesService;
        private readonly IScheduleSource _scheduleSource;
        private readonly INotesService _notesService;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleSource scheduleSource,
            IOutagesService outagesService, INotesService notesService, GraphServiceClient graphClient)
        {
            _logger = logger;
            _graphClient = graphClient;
            _notesService = notesService;
            _scheduleSource = scheduleSource;
            _outagesService = outagesService;
        }

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

            var response = lessons.MapToAnonymousScheduleResponse();
            return Ok(response);
        }

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

            var user = await _graphClient.Me.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Select = new[] { "department" };
            }, token);

            _notesService.ConfigureNotes(lessons, user.Department);

            var response = lessons.MapToPersonalScheduleResponse();
            return Ok(response);
        }
    }
}
