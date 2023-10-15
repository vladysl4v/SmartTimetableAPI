using Microsoft.Graph;

using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class UsersService : IUsersService
{
    private readonly GraphServiceClient _graphClient;
    private readonly IDbRepository _dbRepository;

    public UsersService(GraphServiceClient graphClient, IDbRepository dbRepository)
    {
        _graphClient = graphClient;
        _dbRepository = dbRepository;
    }

    public async Task<UserEntity?> GetUser(CancellationToken token)
    {
        var user = await _graphClient.Me.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Select = new[] { "department", "displayName", "id" };
        }, token);

        if (user.Department is null)
        {
            return null;
        }

        var dbUser = _dbRepository.Get<UserEntity>(x => x.Id == Guid.Parse(user.Id)).SingleOrDefault();
        if (dbUser == null)
        {
            dbUser = new UserEntity
            {
                Id = Guid.Parse(user.Id),
                FullName = user.DisplayName,
                Group = user.Department
            };
            await _dbRepository.Add(dbUser);
            await _dbRepository.SaveChangesAsync(token);
        }
        else
        {
            if (dbUser.FullName != user.DisplayName || dbUser.Group != user.Department)
            {
                dbUser.FullName = user.DisplayName;
                dbUser.Group = user.Department;
                _dbRepository.Update(dbUser);
                await _dbRepository.SaveChangesAsync(token);
            }
        }

        return dbUser;
    }
}