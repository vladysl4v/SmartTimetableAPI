using WebTimetableApi.Models;


namespace WebTimetableApi.Deserializers
{
    public class LessonFactory : FactoryConverter<Lesson, Dictionary<string, string>>
    {
        public override Lesson CreateAndPopulate(Type objectType, Dictionary<string, string> arguments)
        {
            return new Lesson
            {
                Discipline = arguments["discipline"],
                StudyType = arguments["study_type"],
                Cabinet = arguments["cabinet"],
                Teacher = arguments["employee"],
                Subgroup = arguments["study_subgroup"],
                Date = DateOnly.Parse(arguments["full_date"]),
                Start = TimeOnly.Parse(arguments["study_time_begin"]),
                End = TimeOnly.Parse(arguments["study_time_end"])
            };
        }
    }
}
