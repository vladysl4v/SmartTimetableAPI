namespace WebTimetable.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Schedule
    {
        private const string Base = $"{ApiBase}/schedules";

        public const string Get = $"{Base}/{{date}}";
        public const string GetPersonal = $"{Base}/me/{{date}}";
    }

    public static class Settings
    {
        private const string Base = $"{ApiBase}/settings";

        public const string Filters = $"{Base}/filters";
        public const string StudyGroups = $"{Base}/studyGroups";
        public const string OutageGroups = $"{Base}/outageGroups";
    }
}