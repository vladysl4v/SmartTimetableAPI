using System.Linq.Expressions;
using Microsoft.Graph.Models;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

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
        var mockUsersRepo = new Mock<IRepository<UserEntity>>();
        mockUsersRepo.Setup(x => x.Where(It.IsAny<Expression<Func<UserEntity, bool>>>()))
            .Returns(new List<UserEntity> { oldUserInformationInDatabase }.AsQueryable());
        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), mockUsersRepo.Object);
        
        // Act
        var user = await usersService.GetUserAsync(CancellationToken.None);
        
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
        var dbRepositoryMock = new Mock<IRepository<UserEntity>>();
        dbRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<UserEntity, bool>>>()))
            .Returns(new List<UserEntity>().AsQueryable());
        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), dbRepositoryMock.Object);
        
        // Act
        var user = await usersService.GetUserAsync(CancellationToken.None);
        
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
        var dbRepositoryMock = new Mock<IRepository<UserEntity>>();
        dbRepositoryMock.Setup(x => x.Where(It.IsAny<Expression<Func<UserEntity, bool>>>()))
            .Returns(new List<UserEntity>().AsQueryable());
        
        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), dbRepositoryMock.Object);
        
        // Act
        var user = await usersService.GetUserAsync(CancellationToken.None);
        
        // Assert
        user.Should().BeNull();
    }
}