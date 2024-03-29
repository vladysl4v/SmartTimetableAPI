using Microsoft.EntityFrameworkCore;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories.Abstractions;

namespace WebTimetable.Application.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DataContext _context;

    public UsersRepository(DataContext context)
    {
        _context = context;
    }

    public async Task LogOutageUpdateAsync(CancellationToken token)
    {
        await _context.Users.AddAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            FullName = "Outages updated",
            Group = "Admin",
            IsRestricted = false
        }, token);
        await _context.SaveChangesAsync(token);
    }
    
    public async Task<UserEntity> CreateOrUpdateUserAsync(Guid userId, string fullName, string group, CancellationToken token)
    {
        var user = _context.Users.FirstOrDefault(x => x.Id == userId);
        if (user is null)
        {
            user = new UserEntity
            {
                Id = userId,
                FullName = fullName,
                Group = group
            };
            await _context.Users.AddAsync(user, token);
            await _context.SaveChangesAsync(token);
        }
        else
        {
            if (user.FullName == fullName && user.Group == group)
            {
                return user;
            }
            user.FullName = fullName;
            user.Group = group;
        }

        return user;
    }
}