using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;
        private readonly IOutagesService _outagesService;
        public SettingsController(ISettingsService settingsService, IOutagesService outagesService)
        {
            _settingsService = settingsService;
            _outagesService = outagesService;
        }

        [ProducesResponseType(typeof(OutageGroupsResponse), StatusCodes.Status200OK)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Settings.GetOutageGroups)]
        public IActionResult GetOutageGroups()
        {
            var outageGroups = _outagesService.GetOutageGroups();
            var response = outageGroups.MapToOutageGroupsResponse();

            return Ok(response);
        }

        [ProducesResponseType(typeof(FiltersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Settings.GetFilters)]
        public async Task<IActionResult> GetFilters(CancellationToken token)
        {
            var filters = await _settingsService.GetFilters(token);
            var response = filters.MapToFiltersResponse();

            return Ok(response);
        }

        [ProducesResponseType(typeof(StudyGroupsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Settings.GetStudyGroups)]
        public async Task<IActionResult> GetStudyGroups([FromQuery] StudyGroupsRequest request, CancellationToken token)
        {
            var studyGroups = await _settingsService.GetStudyGroups(request.Faculty, request.Course, request.EducationForm, token);
            var response = studyGroups.MapToStudyGroupsResponse();

            return Ok(response);
        }
    }
}
