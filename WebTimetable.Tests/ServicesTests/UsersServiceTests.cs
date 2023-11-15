using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.Graph.Models;
using Moq;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;
using WebTimetable.Tests.TestingUtilities;
using Xunit;

namespace WebTimetable.Tests.ServicesTests;

public class UsersServiceTests
{
    [Fact]
    public async Task UsersService_GetUser_ReturnUpdatedUserEntity()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var oldUserInformationInDatabase = new UserEntity
            { Id = userId, Group = "ObsoleteGroup", FullName = "ObsoleteFullName" };
        var newUserInformation = new User
            { Department = "RelevantGroup", DisplayName = "RelevantFullName", Id = userId.ToString() };
        
        var mockGraphClientFactory = new MockGraphClientFactory()
            .Setup(newUserInformation);
        var dbRepositoryMock = new Mock<IDbRepository>();
        dbRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
            .Returns(new List<UserEntity> { oldUserInformationInDatabase }.AsQueryable());
        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), dbRepositoryMock.Object);
        
        // Act
        var user = await usersService.GetUser(CancellationToken.None);
        
        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(userId);
        user.Group.Should().Be(newUserInformation.Department);
        user.FullName.Should().Be(newUserInformation.DisplayName);
    }
    
    [Fact]
    public async Task UsersService_GetUser_ReturnNewUserEntity()
    {
        // Arrange
        var newUserInformation = new User
            { Department = "Group", DisplayName = "FullName", Id = Guid.NewGuid().ToString() };
        
        var mockGraphClientFactory = new MockGraphClientFactory()
            .Setup(newUserInformation);
        var dbRepositoryMock = new Mock<IDbRepository>();
        dbRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
            .Returns(new List<UserEntity>().AsQueryable());
        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), dbRepositoryMock.Object);
        
        // Act
        var user = await usersService.GetUser(CancellationToken.None);
        
        // Assert
        user.Should().NotBeNull();
        user.Id.Should().Be(newUserInformation.Id);
        user.Group.Should().Be(newUserInformation.Department);
        user.FullName.Should().Be(newUserInformation.DisplayName);
    }
    
    [Fact]
    public async Task UsersService_GetUser_ReturnNull()
    {
        // Arrange
        var newUserInformation = new User
            { Department = null, DisplayName = "FullName", Id = Guid.NewGuid().ToString() };
        
        var mockGraphClientFactory = new MockGraphClientFactory()
            .Setup(newUserInformation);
        var dbRepositoryMock = new Mock<IDbRepository>();
        dbRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<UserEntity, bool>>>()))
            .Returns(new List<UserEntity>().AsQueryable());
        
        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), dbRepositoryMock.Object);
        
        // Act
        var user = await usersService.GetUser(CancellationToken.None);
        
        // Assert
        user.Should().BeNull();
    }
}