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

    public static class Authorization
    {
        private const string Base = $"{ApiBase}/auth";

        public const string Authorize = Base;
    }
}