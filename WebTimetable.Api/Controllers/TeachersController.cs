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
    
    
    [ProducesResponseType(typeof(TeacherScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
    [OutputCache(PolicyName = "ScheduleCache")]
    [HttpGet(ApiEndpoints.Teachers.GetAnonymousSchedule)]
    public async Task<IActionResult> GetAnonymousSchedule([FromRoute] ScheduleRequest request, CancellationToken token, string outageGroup = "")
    {
        var lessons = await _teacherService.GetScheduleAsync(DateTime.Parse(request.Date), request.Identifier, outageGroup, token);
        
        var response = new TeacherScheduleResponse
        {
            Schedule = lessons.ToLessonDTO()
        };
        return Ok(response);
    }

    [ProducesResponseType(typeof(TeacherScheduleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
    [Authorize]
    [RequiredScope("access_as_user")]
    [HttpGet(ApiEndpoints.Teachers.GetIndividualSchedule)]
    public async Task<IActionResult> GetIndividualSchedule([FromRoute] ScheduleRequest request, CancellationToken token, string outageGroup = "")
    {
        var user = await _usersService.GetUserAsync(token);
        if (user is null)
        {
            return Forbid();
        }
        
        var lessons = await _teacherService.GetScheduleAsync(DateTime.Parse(request.Date), request.Identifier, outageGroup, token, user);
        
        var response = new TeacherScheduleResponse
        {
            Schedule = lessons.ToLessonDTO()
        };
        return Ok(response);
    }
    
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