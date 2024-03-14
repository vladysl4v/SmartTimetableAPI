using WebTimetable.Contracts.DataTransferObjects;

namespace WebTimetable.Contracts.Responses;

public class LessonDetailsResponse
{
    public Guid Id { get; init; }
    public List<EventDTO> Meetings { get; init; }
    public List<NoteDTO> Notes { get; init; }
}