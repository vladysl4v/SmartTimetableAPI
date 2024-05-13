using WebTimetable.Application.Models;

namespace WebTimetable.Application.Deserializers;

public class TeacherLessonFactory : FactoryConverter<TeacherLesson, Dictionary<string, string>>
{
    public override TeacherLesson CreateAndPopulate(Type objectType, Dictionary<string, string> arguments)
    {
        return new TeacherLesson
        {
            Discipline = arguments["discipline"],
            StudyType = arguments["study_type"],
            Cabinet = arguments["cabinet"]?.Replace("_", ""),
            StudyGroups = new List<string> { arguments["study_group"] },
            Date = DateOnly.ParseExact(arguments["full_date"], "dd.MM.yyyy"),
            Start = TimeOnly.ParseExact(arguments["study_time_begin"],"HH:mm"),
            End = TimeOnly.ParseExact(arguments["study_time_end"], "HH:mm")
        };
    }
}