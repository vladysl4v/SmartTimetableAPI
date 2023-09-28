using Microsoft.AspNetCore.Mvc;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


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


        [ProducesResponseType(typeof(OutageGroupsResponse), 200)]
        [HttpGet(ApiEndpoints.Settings.OutageGroups)]
        public IActionResult GetOutageGroups()
        {
            var outageGroups = _outagesService.GetOutageGroups();
            var response = outageGroups.MapToOutageGroupsResponse();

            return Ok(response);
        }

        [ProducesResponseType(typeof(FiltersResponse), 200)]
        [HttpGet(ApiEndpoints.Settings.Filters)]
        public async Task<IActionResult> GetFilters()
        {
            var filters = await _settingsService.GetFilters();
            var response = filters.MapToFiltersResponse();

            return Ok(response);
        }

        [ProducesResponseType(typeof(StudyGroupsResponse), 200)]
        [HttpGet(ApiEndpoints.Settings.StudyGroups)]
        public async Task<IActionResult> GetStudyGroups([FromQuery] StudyGroupsRequest request)
        {
            var studyGroups = await _settingsService.GetStudyGroups(request.Faculty, request.Course, request.EducationForm);
            var response = studyGroups.MapToStudyGroupsResponse();

            return Ok(response);
        }
    }
}
