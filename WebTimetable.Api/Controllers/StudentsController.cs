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
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IUsersService _usersService;

        public StudentsController(IStudentService studentService, IUsersService usersService)
        {
            _studentService = studentService;
            _usersService = usersService;
        }

        [ProducesResponseType(typeof(StudentScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "ScheduleCache")]
        [HttpGet(ApiEndpoints.Students.GetSchedule)]
        public async Task<IActionResult> GetSchedule([FromRoute] ScheduleRequest request, CancellationToken token, string outageGroup = "")
        {
            var lessons = await _studentService.GetScheduleAsync(DateTime.Parse(request.Date), request.Identifier, outageGroup, token);
            
            var response = new StudentScheduleResponse
            {
                Schedule = lessons.ToLessonDTO()
            };
            return Ok(response);
        }
        

        [ProducesResponseType(typeof(LessonDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpGet(ApiEndpoints.Students.GetDetails)]
        public async Task<IActionResult> GetLessonDetails([FromRoute] Guid lessonIdentifier, [FromQuery] LessonDetailsRequest request, CancellationToken token)
        {
            var user = await _usersService.GetUserAsync(token);
            if (user is null)
            {
                return Forbid();
            }
            var details = await _studentService.GetLessonDetails(lessonIdentifier, DateOnly.Parse(request.Date),
                TimeOnly.Parse(request.StartTime), TimeOnly.Parse(request.EndTime), user.Group, token);
            var response = details.ToLessonDetailsResponse();
            return Ok(response);
        }
        
        [ProducesResponseType(typeof(StudentFiltersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "FiltersCache")]
        [HttpGet(ApiEndpoints.Students.GetFilters)]
        public async Task<IActionResult> GetFilters(CancellationToken token)
        {
            var filters = await _studentService.GetFiltersAsync(token);
            var response = new StudentFiltersResponse
            {
                Faculties = filters["faculties"],
                Courses = filters["courses"],
                EducForms = filters["educForms"]
            };
            return Ok(response);
        }

        [ProducesResponseType(typeof(FiltersResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "FiltersCache")]
        [HttpGet(ApiEndpoints.Students.GetStudyGroups)]
        public async Task<IActionResult> GetStudyGroups([FromQuery] StudyGroupsRequest request, CancellationToken token)
        {
            var response = new FiltersResponse
            {
                Filters = await _studentService.GetStudyGroupsAsync(request.Faculty, request.Course, request.EducationForm, token)
            };
            return Ok(response);
        }
    }
}