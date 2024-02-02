using Microsoft.Graph.Models;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ApplicationUnitTests.Repositories;

public class UsersRepositoryTests
{
    private readonly MockDataContext _mockDataContext = new();
    
    [Fact]
    public async Task UsersRepository_LogOutageUpdateAsync_AddsNewLog()
    {
        // Arrange
        var usersRepository = new UsersRepository(_mockDataContext);
        
        // Act
        await usersRepository.LogOutageUpdateAsync(CancellationToken.None);
        
        // Assert
        _mockDataContext.Set<UserEntity>().Should().HaveCount(1);
        var log = _mockDataContext.Set<UserEntity>().Single();
        log.FullName.Should().Be("Outages updated");
        log.Group.Should().Be("Admin");
    }

    [Fact]
    public async Task UsersRepository_CreateOrUpdateUserAsync_UpdatesInformation()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var oldUserInformation = new UserEntity { Id = userId, Group = "ObsoleteGroup", FullName = "ObsoleteFullName" };
        
        _mockDataContext.Set<UserEntity>().Add(oldUserInformation);
        await _mockDataContext.SaveChangesAsync();
        var newUserInformation = new { Department = "RelevantGroup", DisplayName = "RelevantFullName" };
        
        var usersRepository = new UsersRepository(_mockDataContext);
        
        // Act
        var user = await usersRepository.CreateOrUpdateUserAsync(userId, newUserInformation.DisplayName,
            newUserInformation.Department, CancellationToken.None);
        
        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
        user.Group.Should().Be(newUserInformation.Department);
        user.FullName.Should().Be(newUserInformation.DisplayName);
        _mockDataContext.Set<UserEntity>().Should().HaveCount(1);
    }
    
    [Fact]
    public async Task UsersRepository_CreateOrUpdateUserAsync_NotUpdatesInformation()
    {
        // Arrange
        var userInformation = new UserEntity { Id = Guid.NewGuid(), Group = "RelevantGroup", FullName = "RelevantFullName" };
        
        _mockDataContext.Set<UserEntity>().Add(userInformation);
        await _mockDataContext.SaveChangesAsync();
        var usersRepository = new UsersRepository(_mockDataContext);
        
        // Act
        var user = await usersRepository.CreateOrUpdateUserAsync(userInformation.Id, userInformation.FullName,
            userInformation.Group, CancellationToken.None);
        
        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(userInformation);
        _mockDataContext.Set<UserEntity>().Should().HaveCount(1);
    }
    
    [Fact]
    public async Task UsersRepository_CreateOrUpdateUserAsync_CreatesUser()
    {
        // Arrange
        _mockDataContext.Set<UserEntity>().Add(new UserEntity { Id = Guid.NewGuid(), Group = "Another Group", FullName = "Another FullName" });
        var usersRepository = new UsersRepository(_mockDataContext);
        var newUserInformation = new { Id = Guid.NewGuid(), Department = "RelevantGroup", DisplayName = "RelevantFullName" };
        
        
        // Act
        var user = await usersRepository.CreateOrUpdateUserAsync(newUserInformation.Id, newUserInformation.DisplayName,
            newUserInformation.Department, CancellationToken.None);
        
        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(newUserInformation.Id);
        user.Group.Should().Be(newUserInformation.Department);
        user.FullName.Should().Be(newUserInformation.DisplayName);
        _mockDataContext.Set<UserEntity>().Should().HaveCount(2);
    }
}