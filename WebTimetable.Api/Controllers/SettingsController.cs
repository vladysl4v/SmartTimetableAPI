using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

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
        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [ProducesResponseType(typeof(OutageGroupsResponse), StatusCodes.Status200OK)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Settings.GetOutageGroups)]
        public IActionResult GetOutageGroups()
        {
            var response = new OutageGroupsResponse
            {
                OutageGroups = _settingsService.GetOutageGroups()
            };
            return Ok(response);
        }

        [ProducesResponseType(typeof(FiltersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Settings.GetFilters)]
        public async Task<IActionResult> GetFilters(CancellationToken token)
        {
            var response = new FiltersResponse
            {
                Filters = await _settingsService.GetFilters(token)
            };
            return Ok(response);
        }

        [ProducesResponseType(typeof(StudyGroupsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Settings.GetStudyGroups)]
        public async Task<IActionResult> GetStudyGroups([FromQuery] StudyGroupsRequest request, CancellationToken token)
        {
            var response = new StudyGroupsResponse
            {
                StudyGroups = await _settingsService.GetStudyGroups(request.Faculty, request.Course, request.EducationForm, token)
            };
            return Ok(response);
        }
    }
}
