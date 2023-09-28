namespace WebTimetable.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Schedule
    {
        public const string GetSchedule = $"{ApiBase}/anonymous/schedule";
        public const string GetPersonalSchedule = $"{ApiBase}/me/schedule";
    }

    public static class Settings
    {
        private const string Base = $"{ApiBase}/settings";

        public const string Filters = $"{Base}/filters";
        public const string StudyGroups = $"{Base}/studyGroups";
        public const string OutageGroups = $"{Base}/outageGroups";
    }
}