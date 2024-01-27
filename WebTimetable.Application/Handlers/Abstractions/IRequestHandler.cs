using WebTimetable.Application.Models;

namespace WebTimetable.Application.Handlers.Abstractions
{
    public interface IRequestHandler
    {
        public Task<Dictionary<string, List<KeyValuePair<string, string>>>> GetStudentFilters(CancellationToken token);

        public Task<List<KeyValuePair<string, string>>> GetStudentStudyGroups(string faculty, int course, int educForm, CancellationToken token);

        public Task<List<StudentLesson>> GetStudentSchedule(DateTime date, string groupId, CancellationToken token);
        
        public Task<List<KeyValuePair<string, string>>> GetTeacherFaculties(CancellationToken token);

        public Task<List<KeyValuePair<string, string>>> GetTeacherChairs(string faculty, CancellationToken token);

        public Task<List<KeyValuePair<string, string>>> GetTeacherEmployees(string faculty, string chair, CancellationToken token);

        public Task<List<TeacherLesson>> GetTeacherSchedule(DateTime date, string teacherId, CancellationToken token);

    }
}