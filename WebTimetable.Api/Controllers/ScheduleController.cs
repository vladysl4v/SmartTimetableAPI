using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Graph;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Models;
using WebTimetable.Application.Schedules.Abstractions;
using WebTimetable.Application.Schedules.Exceptions;
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
            List<Lesson> lessons;
            try
            {
                lessons = await _scheduleSource.GetSchedule(DateTime.Parse(request.StartDate), DateTime.Parse(request.EndDate), request.StudyGroup, token);
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

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpGet(ApiEndpoints.Schedule.GetPersonalSchedule)]
        public async Task<IActionResult> GetPersonalSchedule([FromQuery] PersonalScheduleRequest request, CancellationToken token)
        {
            List<Lesson> lessons;
            try
            {
                lessons = await _scheduleSource.GetSchedule(DateTime.Parse(request.StartDate), DateTime.Parse(request.EndDate), request.StudyGroup, token);
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
