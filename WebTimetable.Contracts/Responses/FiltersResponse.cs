namespace WebTimetable.Contracts.Responses;

public class FiltersResponse
{
    public Dictionary<string, Dictionary<string, string>> Filters { get; init; }
}