namespace WebTimetable.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Teachers
    {
        private const string Base = $"{ApiBase}/teachers";

        public const string GetSchedule = $"{Base}/schedules/{{identifier}}/{{date}}";
        public const string GetDetails = $"{Base}/schedules/details/{{lessonIdentifier}}";
        
        public const string GetFaculties = $"{Base}/faculties";
        public const string GetChairs = $"{Base}/chairs";
        public const string GetEmployees = $"{Base}/employees";
    }

    public static class Cabinets
    {
        private const string Base = $"{ApiBase}/cabinets";

        public const string GetCabinetPath = $"{Base}/{{cabinet}}";
        public const string IsCabinetExists = $"{Base}/{{cabinet}}";
    }

    public static class Students
    {
        private const string Base = $"{ApiBase}/students";
        
        public const string GetSchedule = $"{Base}/schedules/{{identifier}}/{{date}}";
        public const string GetDetails = $"{Base}/schedules/details/{{lessonIdentifier}}";
        
        public const string GetFilters = $"{Base}/filters";
        public const string GetStudyGroups = $"{Base}/studyGroups";
    }

    public static class Settings
    {
        private const string Base = $"{ApiBase}/settings";

        public const string GetOutageGroups = $"{Base}/outageGroups/{{city}}";
    }

    public static class Notes
    {
        private const string Base = $"{ApiBase}/notes";

        public const string AddNote = Base;
        public const string RemoveNote = $"{Base}/{{id:guid}}";
    }
}