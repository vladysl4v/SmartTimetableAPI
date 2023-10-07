using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;

using WebTimetable.Api.Mapping;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;


namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly GraphServiceClient _graphClient;
        private readonly INotesService _notesService;
        public NotesController(GraphServiceClient graphServiceClient, INotesService notesService)
        {
            _graphClient = graphServiceClient;
            _notesService = notesService;
        }

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpPost(ApiEndpoints.Notes.AddNote)]
        public async Task<IActionResult> AddNote([FromBody] AddNoteRequest request, CancellationToken token)
        {
            var user = await _graphClient.Me.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Select = new[] { "department", "displayName", "id" };
            }, token);

            if (user.Department is null)
            {
                return BadRequest("User department cannot be accessed.");
            }

            var note = request.MapToNote(user.Id, user.DisplayName, user.Department);
            await _notesService.AddNoteAsync(note, token);

            return Created(nameof(ScheduleController.GetPersonalSchedule), note.MapToNoteResponse());
        }

        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpDelete(ApiEndpoints.Notes.RemoveNote)]
        public async Task<IActionResult> RemoveNote([FromRoute] Guid id, CancellationToken token)
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
            await _notesService.RemoveNote(note, token);
            return Ok();
        }
    }
}