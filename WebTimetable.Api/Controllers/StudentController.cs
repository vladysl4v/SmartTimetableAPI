using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Identity.Web.Resource;
using WebTimetable.Api.Components;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IUsersService _usersService;

        public StudentController(IStudentService studentService, IUsersService usersService)
        {
            _studentService = studentService;
            _usersService = usersService;
        }

        [ProducesResponseType(typeof(StudentScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "ScheduleCache")]
        [HttpGet(ApiEndpoints.Student.GetSchedule)]
        public async Task<IActionResult> GetAnonymousSchedule([FromQuery] StudentScheduleRequest request, CancellationToken token)
        {
            var lessons = await _studentService.GetScheduleAsync(DateTime.Parse(request.Date), request.StudyGroup, request.OutageGroup, token);
            
            var response = new StudentScheduleResponse
            {
                Schedule = lessons.ToLessonItems()
            };
            return Ok(response);
        }

        [ProducesResponseType(typeof(StudentScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpGet(ApiEndpoints.Student.GetPersonalizedSchedule)]
        public async Task<IActionResult> GetPersonalSchedule([FromQuery] StudentScheduleRequest request, CancellationToken token)
        {
            var user = await _usersService.GetUserAsync(token);
            if (user is null)
            {
                return Forbid();
            }
            
            var lessons = await _studentService.GetScheduleAsync(DateTime.Parse(request.Date), request.StudyGroup, request.OutageGroup, token, user);
            
            var response = new StudentScheduleResponse
            {
                Schedule = lessons.ToLessonItems()
            };
            return Ok(response);
        }
        
        [ProducesResponseType(typeof(StudentFiltersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Student.GetFilters)]
        public async Task<IActionResult> GetFilters(CancellationToken token)
        {
            var response = new StudentFiltersResponse
            {
                Filters = await _studentService.GetFiltersAsync(token)
            };
            return Ok(response);
        }

        [ProducesResponseType(typeof(FilterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "SettingsCache")]
        [HttpGet(ApiEndpoints.Student.GetStudyGroups)]
        public async Task<IActionResult> GetStudyGroups([FromQuery] StudyGroupsRequest request, CancellationToken token)
        {
            var response = new FilterResponse
            {
                Filter = await _studentService.GetStudyGroupsAsync(request.Faculty, request.Course, request.EducationForm, token)
            };
            return Ok(response);
        }
    }
}