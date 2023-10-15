using WebTimetable.Application.Entities;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Mapping;

public static class NotesMappingExtensions
{
    public static NoteEntity MapToNote(this AddNoteRequest request, UserEntity user)
    {
        return new NoteEntity
        {
            LessonId = request.LessonId,
            Message = request.Message,
            Author = user
        };
    }

    public static NoteResponse MapToNoteResponse(this NoteEntity note)
    {
        return new NoteResponse
        {
            NoteId = note.NoteId,
            LessonId = note.LessonId,
            Message = note.Message,
            AuthorId = note.Author.Id,
            AuthorName = note.Author.FullName,
            CreationDate = note.CreationDate
        };
    }
}