namespace WebTimetable.Contracts.Responses;

public class FiltersResponse
{
    public Dictionary<string, List<KeyValuePair<string, string>>> Filters { get; init; }
}