using Riok.Mapperly.Abstractions;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Contracts.DataTransferObjects;
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
    
    public static partial List<StudentLessonDTO> ToLessonDTO(this List<StudentLesson> lessons);
    public static partial List<TeacherLessonDTO> ToLessonDTO(this List<TeacherLesson> lessons);
    
    [MapProperty(nameof(Event.StartTime), nameof(EventDTO.Start))]
    [MapProperty(nameof(Event.EndTime), nameof(EventDTO.End))]
    private static partial EventDTO ToEventDTO(this Event meeting);
    
    [MapProperty(nameof(LessonDetails.Events), nameof(LessonDetailsResponse.Meetings))]
    [MapProperty(nameof(LessonDetails.Notes), nameof(LessonDetailsResponse.Notes))]
    public static partial LessonDetailsResponse ToLessonDetailsResponse(this LessonDetails details);
    
    private static partial StudentLessonDTO ToLessonDTO(this StudentLesson studentLesson);
    private static partial TeacherLessonDTO ToLessonDTO(this TeacherLesson studentLesson);
    private static partial NoteEntity MapToNoteEntity(this AddNoteRequest request);
    
    [MapProperty(nameof(@NoteEntity.Author.FullName), nameof(NoteDTO.AuthorName))]
    private static partial NoteDTO ToNoteDTO(this NoteEntity note);
    private static partial OutageDTO ToOutageDTO(this Outage outage);
}