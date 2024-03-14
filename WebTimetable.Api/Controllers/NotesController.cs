using Asp.Versioning;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using WebTimetable.Api.Components;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Controllers
{
    [ApiVersion(1.0)]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesService _notesService;
        private readonly IUsersService _usersService;
        public NotesController(INotesService notesService, IUsersService usersService)
        {
            _notesService = notesService;
            _usersService = usersService;
        }

        /// <summary>
        /// Authorized only. Creates a new note for the lesson in user group.
        /// </summary>
        [ProducesResponseType(typeof(NoteResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        [RequiredScope("access_as_user")]
        [HttpPost(ApiEndpoints.Notes.AddNote)]
        public async Task<IActionResult> AddNote([FromBody] AddNoteRequest request, CancellationToken token)
        {
            var user = await _usersService.GetUserAsync(token);
            if (user is null)
            {
                return Unauthorized("User department cannot be accessed by server.");
            }

            if (user.IsRestricted)
            {
                return Forbid();
            }

            var note = request.ToNoteEntity(user);
            var isSuccessful = await _notesService.AddNoteAsync(note, token);
            if (!isSuccessful)
            {
                return Conflict();
            }
            
            return Created("", note.ToNoteResponse());
        }

        /// <summary>
        /// Authorized only. Removes user's note from the lesson.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                return Forbid();
            }
            await _notesService.RemoveNote(note, token);
            return Ok();
        }
    }
}