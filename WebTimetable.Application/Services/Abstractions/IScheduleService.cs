using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;


namespace WebTimetable.Application.Services.Abstractions;

public interface IScheduleService
{
    public Task<List<Lesson>?> GetPersonalSchedule(DateTime start, DateTime end, string studyGroup, int outageGroup, UserEntity? user,
        CancellationToken token);
    public Task<List<Lesson>> GetGuestSchedule(DateTime start, DateTime end, string studyGroup, int outageGroup, CancellationToken token);
}