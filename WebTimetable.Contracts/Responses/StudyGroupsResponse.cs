namespace WebTimetable.Contracts.Responses;

public class StudyGroupsResponse
{
    public required Dictionary<string, string> StudyGroups { get; init; }
}