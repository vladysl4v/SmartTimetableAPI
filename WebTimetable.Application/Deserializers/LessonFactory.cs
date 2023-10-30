using WebTimetable.Application.Models;


namespace WebTimetable.Application.Deserializers
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
                Date = DateOnly.ParseExact(arguments["full_date"], "dd.MM.yyyy"),
                Start = TimeOnly.ParseExact(arguments["study_time_begin"],"HH:mm"),
                End = TimeOnly.ParseExact(arguments["study_time_end"], "HH:mm")
            };
        }
    }
}
