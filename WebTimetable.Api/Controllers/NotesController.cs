using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;


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

    [HttpPost(ApiEndpoints.Notes.AddNote)]
    public async Task<IActionResult> AddNote([FromBody] AddNoteRequest request)
    {
        User? user;
        try
        {
            user = await _graphClient.Me.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Select = new[] { "department", "displayName", "id" };
            });
        }
        catch (Exception ex)
        {
            return StatusCode(502, "Error occurred while retrieving the Microsoft information profile. Details: " + ex.Message);
        }

        if (user.Id is null || user.DisplayName is null || user.Department is null)
        {
            return BadRequest();
        }

        var note = request.MapToNote(user.Id, user.DisplayName, user.Department);
        await _notesService.AddNoteAsync(note);

        return Created(nameof(ScheduleController.GetPersonalSchedule), note.MapToNoteResponse());
    }

    [HttpDelete(ApiEndpoints.Notes.RemoveNote)]
    public async Task<IActionResult> RemoveNote([FromRoute] Guid id)
    {
        var note = _notesService.GetNoteById(id);
        if (note is null)
        {
            return NotFound();
        }
        if (note.AuthorId != Guid.Parse(User.GetObjectId()!))
        {
            return Unauthorized();
        }
        await _notesService.RemoveNote(note);
        return Ok();
    }
}