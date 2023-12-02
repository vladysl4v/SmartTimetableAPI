using Microsoft.Graph;

using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class UsersService : IUsersService
{
    private readonly GraphServiceClient _graphClient;
    private readonly IRepository<UserEntity> _users;

    public UsersService(GraphServiceClient graphClient, IRepository<UserEntity> users)
    {
        _graphClient = graphClient;
        _users = users;
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

        var dbUser = _users.Where(x => x.Id == Guid.Parse(user.Id)).SingleOrDefault();
        if (dbUser == null)
        {
            dbUser = new UserEntity
            {
                Id = Guid.Parse(user.Id),
                FullName = user.DisplayName,
                Group = user.Department
            };
            await _users.AddAsync(dbUser, token);
            await _users.SaveChangesAsync(token);
        }
        else
        {
            if (dbUser.FullName != user.DisplayName || dbUser.Group != user.Department)
            {
                dbUser.FullName = user.DisplayName;
                dbUser.Group = user.Department;
                _users.Update(dbUser);
                await _users.SaveChangesAsync(token);
            }
        }

        return dbUser;
    }
}