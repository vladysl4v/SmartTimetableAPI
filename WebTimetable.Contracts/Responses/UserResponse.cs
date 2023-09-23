namespace WebTimetable.Contracts.Responses;

public class UserResponse
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string StudyGroupId { get; set; }
    public int OutagesGroup { get; set; }
}