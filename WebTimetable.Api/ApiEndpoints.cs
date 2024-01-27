namespace WebTimetable.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Teacher
    {
        private const string Base = $"{ApiBase}/teacher";
        
        public const string GetSchedule = $"{Base}/schedule";
        public const string GetPersonalizedSchedule = $"{Base}/schedule/personalized";
        
        public const string GetFaculties = $"{Base}/faculties";
        public const string GetChairs = $"{Base}/chairs";
        public const string GetEmployees = $"{Base}/employees";
    }

    public static class Student
    {
        private const string Base = $"{ApiBase}/student";
        
        public const string GetSchedule = $"{Base}/schedule";
        public const string GetPersonalizedSchedule = $"{Base}/schedule/personalized";
        
        public const string GetFilters = $"{Base}/filters";
        public const string GetStudyGroups = $"{Base}/studyGroups";
    }

    public static class Settings
    {
        private const string Base = $"{ApiBase}/settings";

        public const string GetOutageGroups = $"{Base}/outageGroups";
    }

    public static class Notes
    {
        private const string Base = $"{ApiBase}/notes";

        public const string AddNote = Base;
        public const string RemoveNote = $"{Base}/{{id:guid}}";
    }
}