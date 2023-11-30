using Microsoft.Extensions.Logging;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Commands;
using WebTimetable.Tests.TestingUtilities;

namespace WebTimetable.Tests.ServicesTests.CommandsTests;

public class UpdateOutagesCommandTests
{
    [Fact]
    public async Task UpdateOutagesCommand_Execute_SuccessfullyFills()
    {
        // Arrange
        var data = "\"data\":{\"1\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}},\"2\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}}}}\n\r\n\"sch_names\":{\"1\":\"Group 1\",\"2\":\"Group 2\",\"3\":\"Group 3\",\"4\":\"Group 4\",\"5\":\"Group 5\",\"6\":\"Group 6\"}";
    
        var mockDbRepository = new Mock<IDbRepository>();
        mockDbRepository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Verifiable();
        var mockHttpFactory = new MockHttpFactory().Setup(data);
        var mockLogger = new Mock<ILogger<UpdateOutagesCommand>>();
        var command = new UpdateOutagesCommand(mockDbRepository.Object, mockHttpFactory, mockLogger.Object);
        
        // Act
        await command.Execute(null);
        
        //Assert
        mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>((level) => level == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Never);
        mockDbRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateOutagesCommand_Execute_EmptyResponse()
    {
        // Arrange
        var data = "";
        var mockDbRepository = new Mock<IDbRepository>();
        mockDbRepository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Verifiable();
        var mockHttpFactory = new MockHttpFactory().Setup(data);
        var mockLogger = new Mock<ILogger<UpdateOutagesCommand>>();
        var command = new UpdateOutagesCommand(mockDbRepository.Object, mockHttpFactory, mockLogger.Object);
        
        // Act
        await command.Execute(null);
        
        //Assert
        mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>((level) => level == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Exactly(2));
        mockDbRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateOutagesCommand_Execute_ResponseWithEmptyData()
    {
        // Arrange
        var data = "\"data\":{\"1\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}},\"2\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}}}}\n\r\n\"sch_names\":{}";
    
        var mockDbRepository = new Mock<IDbRepository>();
        mockDbRepository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Verifiable();
        var mockHttpFactory = new MockHttpFactory().Setup(data);
        var mockLogger = new Mock<ILogger<UpdateOutagesCommand>>();
        var command = new UpdateOutagesCommand(mockDbRepository.Object, mockHttpFactory, mockLogger.Object);
        
        // Act
        await command.Execute(null);
        
        //Assert
        mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>((level) => level == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);
        mockDbRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateOutagesCommand_Execute_CantFillDatabase()
    {
        // Arrange
        var data = "\"data\":{\"1\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}},\"2\":{\"1\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"2\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"3\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"4\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"},\"5\":{\"1\":\"maybe\",\"2\":\"yes\",\"3\":\"yes\",\"4\":\"no\",\"5\":\"no\",\"6\":\"no\",\"7\":\"no\",\"8\":\"maybe\",\"9\":\"maybe\",\"10\":\"maybe\",\"11\":\"yes\",\"12\":\"yes\",\"13\":\"no\",\"14\":\"no\",\"15\":\"no\",\"16\":\"no\",\"17\":\"maybe\",\"18\":\"maybe\",\"19\":\"maybe\",\"20\":\"yes\",\"21\":\"yes\",\"22\":\"no\",\"23\":\"no\",\"24\":\"no\"},\"6\":{\"1\":\"no\",\"2\":\"maybe\",\"3\":\"maybe\",\"4\":\"maybe\",\"5\":\"yes\",\"6\":\"yes\",\"7\":\"no\",\"8\":\"no\",\"9\":\"no\",\"10\":\"no\",\"11\":\"maybe\",\"12\":\"maybe\",\"13\":\"maybe\",\"14\":\"yes\",\"15\":\"yes\",\"16\":\"no\",\"17\":\"no\",\"18\":\"no\",\"19\":\"no\",\"20\":\"maybe\",\"21\":\"maybe\",\"22\":\"maybe\",\"23\":\"yes\",\"24\":\"yes\"},\"7\":{\"1\":\"no\",\"2\":\"no\",\"3\":\"no\",\"4\":\"no\",\"5\":\"maybe\",\"6\":\"maybe\",\"7\":\"maybe\",\"8\":\"yes\",\"9\":\"yes\",\"10\":\"no\",\"11\":\"no\",\"12\":\"no\",\"13\":\"no\",\"14\":\"maybe\",\"15\":\"maybe\",\"16\":\"maybe\",\"17\":\"yes\",\"18\":\"yes\",\"19\":\"no\",\"20\":\"no\",\"21\":\"no\",\"22\":\"no\",\"23\":\"maybe\",\"24\":\"maybe\"}}}}\n\r\n\"sch_names\":{\"1\":\"Group 1\",\"2\":\"Group 2\",\"3\":\"Group 3\",\"4\":\"Group 4\",\"5\":\"Group 5\",\"6\":\"Group 6\"}";
    
        var mockDbRepository = new Mock<IDbRepository>();
        mockDbRepository.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<It.IsAnyType>>())).Throws<Exception>();
        mockDbRepository.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Verifiable();
        var mockHttpFactory = new MockHttpFactory().Setup(data);
        var mockLogger = new Mock<ILogger<UpdateOutagesCommand>>();
        var command = new UpdateOutagesCommand(mockDbRepository.Object, mockHttpFactory, mockLogger.Object);
        
        // Act
        await command.Execute(null);
        
        //Assert
        mockLogger.Verify(
            x => x.Log(
                It.Is<LogLevel>((level) => level == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);
        mockDbRepository.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}