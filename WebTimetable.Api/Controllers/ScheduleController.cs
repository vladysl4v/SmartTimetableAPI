﻿using Asp.Versioning;

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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly IUsersService _usersService;

        public ScheduleController(IScheduleService scheduleService, IUsersService usersService)
        {
            _scheduleService = scheduleService;
            _usersService = usersService;
        }

        [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [OutputCache(PolicyName = "ScheduleCache")]
        [HttpGet(ApiEndpoints.Schedule.GetAnonymousSchedule)]
        public async Task<IActionResult> GetAnonymousSchedule([FromQuery] AnonymousScheduleRequest request, CancellationToken token)
        {
            var lessons = await _scheduleService.GetGuestSchedule(DateTime.Parse(request.Date), request.StudyGroup, request.OutageGroup, token);
            
            var response = new ScheduleResponse
            {
                Schedule = lessons.ToLessonItems()
            };
            return Ok(response);
        }

        [ProducesResponseType(typeof(ScheduleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpGet(ApiEndpoints.Schedule.GetPersonalSchedule)]
        public async Task<IActionResult> GetPersonalSchedule([FromQuery] PersonalScheduleRequest request, CancellationToken token)
        {
            var user = await _usersService.GetUser(token);
            var lessons = await _scheduleService.GetPersonalSchedule(DateTime.Parse(request.Date), request.StudyGroup, request.OutageGroup, user, token);
            if (lessons is null)
            {
                return Forbid();
            }

            var response = new ScheduleResponse
            {
                Schedule = lessons.ToLessonItems()
            };
            return Ok(response);
        }
    }
}