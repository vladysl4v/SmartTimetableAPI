using Riok.Mapperly.Abstractions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Contracts.Models;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Api.Components;

[Mapper]
public static partial class MappingExtensions
{
    public static NoteEntity ToNoteEntity(this AddNoteRequest note, UserEntity author)
    {
        var noteEntity = note.MapToNoteEntity();
        noteEntity.Author = author;
        return noteEntity;
    }
    
    [MapProperty(nameof(@NoteEntity.Author.FullName), nameof(NoteResponse.AuthorName))]
    public static partial NoteResponse ToNoteResponse(this NoteEntity note);
    
    public static partial List<StudentLessonItem> ToLessonItems(this List<StudentLesson> lessons);
    public static partial List<TeacherLessonItem> ToLessonItems(this List<TeacherLesson> lessons);
    
    [MapProperty(nameof(Event.StartTime), nameof(EventItem.Start))]
    [MapProperty(nameof(Event.EndTime), nameof(EventItem.End))]
    private static partial EventItem ToEventItem(this Event meeting);
    
    [MapProperty(nameof(StudentLesson.Events), nameof(StudentLessonItem.Meetings))]
    private static partial StudentLessonItem ToLessonItem(this StudentLesson studentLesson);
    private static partial TeacherLessonItem ToLessonItem(this TeacherLesson studentLesson);
    private static partial NoteEntity MapToNoteEntity(this AddNoteRequest request);
    private static partial NoteItem ToNoteItem(this Note note);
    private static partial OutageItem ToOutageItem(this Outage outage);
}