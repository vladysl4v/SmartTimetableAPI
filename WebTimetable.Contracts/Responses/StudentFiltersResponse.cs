namespace WebTimetable.Contracts.Responses;

public class StudentFiltersResponse
{
    public List<KeyValuePair<string, string>> Faculties { get; init; }
    public List<KeyValuePair<string, string>> Courses { get; init; }
    public List<KeyValuePair<string, string>> EducForms { get; init; }
}