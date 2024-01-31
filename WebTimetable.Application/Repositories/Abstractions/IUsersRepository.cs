using WebTimetable.Application.Entities;

namespace WebTimetable.Application.Repositories.Abstractions;

public interface IUsersRepository
{
    public Task LogOutageUpdateAsync(CancellationToken token);

    public Task<UserEntity> CreateOrUpdateUserAsync(Guid userId, string fullName, string group, CancellationToken token);
    
}