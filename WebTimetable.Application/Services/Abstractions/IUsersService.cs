using WebTimetable.Application.Entities;


namespace WebTimetable.Application.Services.Abstractions;

public interface IUsersService
{
    public Task<UserEntity?> GetUser(CancellationToken token);

}