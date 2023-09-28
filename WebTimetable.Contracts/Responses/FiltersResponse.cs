namespace WebTimetable.Contracts.Responses;

public class FiltersResponse
{
    public required Dictionary<string, Dictionary<string, string>> Filters { get; init; }
}