using Microsoft.Graph;

using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class UsersService : IUsersService
{
    private readonly GraphServiceClient _graphClient;
    private readonly IUsersRepository _usersRepository;

    public UsersService(GraphServiceClient graphClient, IUsersRepository usersRepository)
    {
        _graphClient = graphClient;
        _usersRepository = usersRepository;
    }

    public async Task<UserEntity?> GetUserAsync(CancellationToken token)
    {
        var user = await _graphClient.Me.GetAsync((requestConfiguration) =>
        {
            requestConfiguration.QueryParameters.Select = new[] { "department", "displayName", "id" };
        }, token);

        if (user.Department is null)
        {
            return null;
        }
        
        var userEntity =
            await _usersRepository.CreateOrUpdateUserAsync(Guid.Parse(user.Id), user.DisplayName, user.Department,
                token);

        return userEntity;
    }
}