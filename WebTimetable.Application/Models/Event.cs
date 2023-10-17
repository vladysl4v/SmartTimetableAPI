namespace WebTimetable.Application.Models;

public class Event
{
    public DateOnly Date { get; set; }
    public string Title { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Link { get; set; }
}