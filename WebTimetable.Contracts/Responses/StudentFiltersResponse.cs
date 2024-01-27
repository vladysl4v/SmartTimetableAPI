namespace WebTimetable.Contracts.Responses;

public class StudentFiltersResponse
{
    public Dictionary<string, List<KeyValuePair<string, string>>> Filters { get; init; }
}