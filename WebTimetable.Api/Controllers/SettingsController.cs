using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet(ApiEndpoints.Settings.Filters)]
        public async Task<IActionResult> GetFilters()
        {
            var request = await _settingsService.GetFilters();
            if (request == null)
            {
                return BadRequest();
            }
            return Ok(request);
        }

        [HttpGet(ApiEndpoints.Settings.OutageGroups)]
        public IActionResult GetOutages()
        {
            var request = _outagesService.GetOutageGroups();
            if (request == null)
            {
                return BadRequest();
            }
            return Ok(request);
        }

        [HttpGet(ApiEndpoints.Settings.StudyGroups)]
        public async Task<IActionResult> GetStudyGroups(string faculty, string course, string educForm)
        {
            var output = await _settingsService.GetStudyGroups(faculty, course, educForm);
            if (output == null)
            {
                return BadRequest();
            }
            return Ok(output);
        }
    }
}
