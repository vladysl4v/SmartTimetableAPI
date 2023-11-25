using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;


namespace WebTimetable.Application.Services.Abstractions;

public interface IScheduleService
{
    public Task<List<Lesson>?> GetPersonalSchedule(DateTime date, string studyGroup, string outageGroup,
        UserEntity? user, CancellationToken token);
    public Task<List<Lesson>> GetGuestSchedule(DateTime date, string studyGroup, string outageGroup,
        CancellationToken token);
}