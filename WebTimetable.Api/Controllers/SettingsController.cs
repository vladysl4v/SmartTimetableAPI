using Microsoft.AspNetCore.Mvc;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;


namespace WebTimetable.Api.Controllers
{
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

        [HttpGet(ApiEndpoints.Settings.GetOutageGroups)]
        public IActionResult GetOutageGroups()
        {
            var outageGroups = _outagesService.GetOutageGroups();
            var response = outageGroups.MapToOutageGroupsResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Settings.GetFilters)]
        public async Task<IActionResult> GetFilters(CancellationToken token)
        {
            var filters = await _settingsService.GetFilters(token);
            var response = filters.MapToFiltersResponse();

            return Ok(response);
        }

        [HttpGet(ApiEndpoints.Settings.GetStudyGroups)]
        public async Task<IActionResult> GetStudyGroups([FromQuery] StudyGroupsRequest request, CancellationToken token)
        {
            var studyGroups = await _settingsService.GetStudyGroups(request.Faculty, request.Course, request.EducationForm, token);
            var response = studyGroups.MapToStudyGroupsResponse();

            return Ok(response);
        }
    }
}
