namespace WebTimetable.Application.Schedules.Exceptions
{
    public class ScheduleNotLoadedException : Exception
    {
        private readonly string _studyGroup;

        public ScheduleNotLoadedException(string message, string studyGroup) : base(message)
        {
            _studyGroup = studyGroup;
        }

        public ScheduleNotLoadedException(Exception innerExpection, string studyGroup, string message) : base(message, innerExpection)
        {
            _studyGroup = studyGroup;
        }

        public override string ToString()
        {
            return "Error occured during retrieving schedule. Study group: " + _studyGroup + "\n" + base.ToString();
        }
    }
}
