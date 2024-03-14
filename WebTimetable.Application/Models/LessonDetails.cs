using WebTimetable.Application.Entities;

namespace WebTimetable.Application.Models;

public class LessonDetails
{
    public Guid Id { get; set; }
    public List<Event> Events { get; set; }
    public List<NoteEntity> Notes { get; set; }
}