using System.Linq.Expressions;
using Microsoft.Graph.Models;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Repositories.Abstractions;
using WebTimetable.Application.Services;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ApplicationUnitTests.Services;

public class UsersServiceTests
{
    [Fact]
    public async Task UsersService_GetUser_ReturnNewUserEntity()
    {
        // Arrange
        var newUserInformation = new User
            { Department = "Group", DisplayName = "FullName", Id = Guid.NewGuid().ToString() };
        
        var mockGraphClientFactory = new MockGraphClientFactory()
            .Setup(newUserInformation);
        var dbRepositoryMock = new Mock<IUsersRepository>();
        dbRepositoryMock.Setup(x => x.CreateOrUpdateUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();

        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), dbRepositoryMock.Object);
        
        // Act
        await usersService.GetUserAsync(CancellationToken.None);
        
        // Assert
        dbRepositoryMock.Verify(x => x.CreateOrUpdateUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UsersService_GetUser_ReturnNull()
    {
        // Arrange
        var newUserInformation = new User
            { Department = null, DisplayName = "FullName", Id = Guid.NewGuid().ToString() };
        
        var mockGraphClientFactory = new MockGraphClientFactory()
            .Setup(newUserInformation);
        var dbRepositoryMock = new Mock<IUsersRepository>();
        dbRepositoryMock.Setup(x => x.CreateOrUpdateUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Verifiable();
        
        var usersService = new UsersService(mockGraphClientFactory.CreateClient(), dbRepositoryMock.Object);
        
        // Act
        var user = await usersService.GetUserAsync(CancellationToken.None);
        
        // Assert
        user.Should().BeNull();
        dbRepositoryMock.Verify(x => x.CreateOrUpdateUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}