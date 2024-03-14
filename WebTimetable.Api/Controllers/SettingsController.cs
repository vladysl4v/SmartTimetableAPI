using Asp.Versioning;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using WebTimetable.Application.Services.Abstractions;
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
        
        /// <summary>
        /// Retrieves the list of outage groups for the specified city.
        /// </summary>
        [ProducesResponseType(typeof(StudentFiltersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "FiltersCache")]
        [HttpGet(ApiEndpoints.Settings.GetOutageGroups)]
        public IActionResult GetOutageGroups([FromRoute] string city)
        {
            var response = new FiltersResponse
            {
                Filters = _settingsService.GetOutageGroups(city)
            };
            return Ok(response);
        }
    }
}