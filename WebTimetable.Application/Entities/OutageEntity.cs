using WebTimetable.Application.Models;

namespace WebTimetable.Application.Entities;

public class OutageEntity
{
    public string City { get; set; }
    public string Group { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public List<Outage> Outages { get; set; }
}