using WebTimetable.Contracts.Responses;


namespace WebTimetable.Api.Mapping;

public static class SettingsMappingExtensions
{
    public static FiltersResponse MapToFiltersResponse(this Dictionary<string, Dictionary<string, string>> filters)
    {
        return new FiltersResponse
        {
            Filters = filters
        };
    }

    public static OutageGroupsResponse MapToOutageGroupsResponse(this List<string> outageGroups)
    {
        return new OutageGroupsResponse
        {
            OutageGroups = outageGroups
        };
    }

    public static StudyGroupsResponse MapToStudyGroupsResponse(this Dictionary<string, string> studyGroups)
    {
        return new StudyGroupsResponse
        {
            StudyGroups = studyGroups
        };
    }
}