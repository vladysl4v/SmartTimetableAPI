using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Controllers;


[Authorize]
[ApiController]
[RequiredScope("access_as_user")]
public class NotesController : ControllerBase
{
    private readonly GraphServiceClient _graphClient;
    private readonly INotesService _notesService;
    public NotesController(GraphServiceClient graphServiceClient, INotesService notesService)
    {
        _graphClient = graphServiceClient;
        _notesService = notesService;
    }

    [ProducesResponseType(400)]
    [ProducesResponseType(502)]
    [ProducesResponseType(typeof(NoteResponse), 201)]
    [HttpPost(ApiEndpoints.Notes.AddNote)]
    public async Task<IActionResult> AddNote([FromBody]AddNoteRequest request)
    {
        string? userStudyGroup;
        try
        {
            var user = await _graphClient.Me.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Select = new[] { "department" };
            });
            userStudyGroup = user?.Department;
        }
        catch (Exception ex)
        {
            return StatusCode(502, "Error occurred while retrieving the Microsoft information profile. Details: " + ex.Message);
        }

        if (User.GetObjectId() is null || User.GetDisplayName() is null || userStudyGroup is null)
        {
            return BadRequest();
        }

        var note = request.MapToNote(User.GetObjectId()!, User.GetDisplayName()!, userStudyGroup);

        await _notesService.AddNoteAsync(note);

        return Created(nameof(ScheduleController.GetAnonymousSchedule), note.MapToNoteResponse());
    }
}