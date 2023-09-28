using WebTimetable.Application.Entities;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Mapping;

public static class NotesMappingExtensions
{
    public static NoteEntity MapToNote(this AddNoteRequest request, string authorId, string authorFullName, string authorGroup)
    {
        return new NoteEntity
        {
            LessonId = request.LessonId,
            Message = request.Message,
            AuthorId = Guid.Parse(authorId),
            AuthorName = ShortenFullName(authorFullName),
            AuthorGroup = authorGroup
        };
    }

    public static NoteResponse MapToNoteResponse(this NoteEntity note)
    {
        return new NoteResponse
        {
            NoteId = note.NoteId,
            LessonId = note.LessonId,
            Message = note.Message,
            AuthorId = note.AuthorId,
            AuthorName = note.AuthorName,
            CreationDate = note.CreationDate
        };
    }

    private static string ShortenFullName(string employee)
    {
        if (string.IsNullOrEmpty(employee))
            return employee;

        string[] EmplSplitted = employee.Split();
        // if the string is not of format "Surname Name Patronymic"
        return EmplSplitted.Length != 3 ? employee : $"{EmplSplitted[0]} {EmplSplitted[1][0]}.{EmplSplitted[2][0]}.";
    }
}