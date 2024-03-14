using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Identity.Web.Resource;
using WebTimetable.Api.Components;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Api.Controllers;

public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly IUsersService _usersService;

    public TeachersController(ITeacherService teacherService, IUsersService usersService)
    {
        _teacherService = teacherService;
        _usersService = usersService;
    }
    
    /// <summary>
    /// Returns the teacher schedule for the specified teacher identifier and date.
    /// </summary>
    /// <param name="outageGroup">Optional. The group for which to obtain the outage schedule.</param>
    [ProducesResponseType(typeof(TeacherScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
    [OutputCache(PolicyName = "ScheduleCache")]
    [HttpGet(ApiEndpoints.Teachers.GetSchedule)]
    public async Task<IActionResult> GetSchedule([FromRoute] ScheduleRequest request, CancellationToken token, string outageGroup = "")
    {
        var lessons = await _teacherService.GetScheduleAsync(DateTime.Parse(request.Date), request.Identifier, outageGroup, token);
        
        var response = new TeacherScheduleResponse
        {
            Schedule = lessons.ToLessonDTO()
        };
        return Ok(response);
    }

    /// <summary>
    /// Authorized only. Returns the personalized notes and online-meetings of the specified lesson.
    /// </summary>
    /// <param name="lessonIdentifier">Identifier of lesson.</param>
    /// <returns></returns>
    [ProducesResponseType(typeof(LessonDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    [RequiredScope("access_as_user")]
    [HttpGet(ApiEndpoints.Teachers.GetDetails)]
    public async Task<IActionResult> GetLessonDetails([FromRoute] Guid lessonIdentifier, [FromQuery] LessonDetailsRequest request, CancellationToken token)
    {
        var user = await _usersService.GetUserAsync(token);
        if (user is null)
        {
            return Forbid();
        }
        var details = await _teacherService.GetLessonDetails(lessonIdentifier, DateOnly.Parse(request.Date),
            TimeOnly.Parse(request.StartTime), TimeOnly.Parse(request.EndTime), user.Group, token);
        var response = details.ToLessonDetailsResponse();
        return Ok(response);
    }
    
    /// <summary>
    /// Returns the filters for the configuration of teachers schedule.
    /// </summary>
    [ProducesResponseType(typeof(StudentFiltersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
    [OutputCache(PolicyName = "FiltersCache")]
    [HttpGet(ApiEndpoints.Teachers.GetFaculties)]
    public async Task<IActionResult> GetFaculties(CancellationToken token)
    {
        var response = new FiltersResponse
        {
            Filters = await _teacherService.GetFacultiesAsync(token)
        };
        return Ok(response);
    }

    /// <summary>
    /// Returns the chairs for the specified faculty.
    /// </summary>
    [ProducesResponseType(typeof(FiltersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
    [OutputCache(PolicyName = "FiltersCache")]
    [HttpGet(ApiEndpoints.Teachers.GetChairs)]
    public async Task<IActionResult> GetChairs([FromQuery] ChairsRequest request, CancellationToken token)
    {
        var response = new FiltersResponse
        {
            Filters = await _teacherService.GetChairsAsync(request.Faculty, token)
        };
        return Ok(response);
    }
    
    /// <summary>
    /// Returns the employees for the specified faculty and chair.
    /// </summary>
    [ProducesResponseType(typeof(FiltersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
    [OutputCache(PolicyName = "FiltersCache")]
    [HttpGet(ApiEndpoints.Teachers.GetEmployees)]
    public async Task<IActionResult> GetEmployees([FromQuery] EmployeesRequest request, CancellationToken token)
    {
        var response = new FiltersResponse
        {
            Filters = await _teacherService.GetEmployeesAsync(request.Faculty, request.Chair, token)
        };
        return Ok(response);
    }
}