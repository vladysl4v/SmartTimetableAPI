namespace WebTimetable.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Schedule
    {
        private const string Base = $"{ApiBase}/schedule";

        public const string GetAnonymousSchedule = $"{Base}/guest";
        public const string GetPersonalSchedule = $"{Base}/personal";
    }
    
    public static class Settings
    {
        private const string Base = $"{ApiBase}/settings";

        public const string GetFilters = $"{Base}/filters";
        public const string GetStudyGroups = $"{Base}/studyGroups";
        public const string GetOutageGroups = $"{Base}/outageGroups";
    }

    public static class Notes
    {
        private const string Base = $"{ApiBase}/notes";

        public const string AddNote = $"{Base}";
        public const string RemoveNote = $"{Base}/{{id:guid}}";
    }
}