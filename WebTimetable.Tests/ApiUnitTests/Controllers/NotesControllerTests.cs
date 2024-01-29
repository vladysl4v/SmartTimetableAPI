using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTimetable.Api.Controllers;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Services.Abstractions;
using WebTimetable.Contracts.Requests;
using WebTimetable.Contracts.Responses;

namespace WebTimetable.Tests.ApiUnitTests.Controllers;

public class NotesControllerTests
{
    [Fact]
    public async Task NotesController_AddNote_ReturnsOk()
    {
        // Arrange
        var note = new AddNoteRequest
        {
            LessonId = Guid.NewGuid(),
            Message = "Test message"
        };
        var mockNotesService = new Mock<INotesService>();
        mockNotesService.Setup(x => x.AddNoteAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                FullName = "Test user",
                Group = "Test group",
                IsRestricted = false
            });
        var controller = new NotesController(mockNotesService.Object, mockUsersService.Object);

        // Act
        var result = await controller.AddNote(note, CancellationToken.None);

        // Assert
        result.Should().BeOfType<CreatedResult>();
        ((CreatedResult)result).Value.Should().BeOfType<NoteResponse>();
        var resultNote = (NoteResponse)((CreatedResult)result).Value!;
        resultNote.Message.Should().Be(note.Message);
        resultNote.LessonId.Should().Be(note.LessonId);
    }
    
    [Fact]
    public async Task NotesController_AddNote_ReturnsForbid()
    {
        // Arrange
        var note = new AddNoteRequest
        {
            LessonId = Guid.NewGuid(),
            Message = "Test message"
        };
        var mockNotesService = new Mock<INotesService>();
        mockNotesService.Setup(x => x.AddNoteAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()))
            .Verifiable();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserEntity() { IsRestricted = true });
        var controller = new NotesController(mockNotesService.Object, mockUsersService.Object);

        // Act
        var result = await controller.AddNote(note, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ForbidResult>();
        mockNotesService.Verify(x => x.AddNoteAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task NotesController_AddNote_ReturnsConflict()
    {
        // Arrange
        var note = new AddNoteRequest
        {
            LessonId = Guid.NewGuid(),
            Message = "Test message"
        };
        var mockNotesService = new Mock<INotesService>();
        mockNotesService.Setup(x => x.AddNoteAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserEntity
            {
                Id = Guid.NewGuid(),
                FullName = "Test user",
                Group = "Test group",
                IsRestricted = false
            });
        var controller = new NotesController(mockNotesService.Object, mockUsersService.Object);

        // Act
        var result = await controller.AddNote(note, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ConflictResult>();
    }
    
    [Fact]
    public async Task NotesController_AddNote_ReturnsUnauthorized()
    {
        // Arrange
        var note = new AddNoteRequest
        {
            LessonId = Guid.NewGuid(),
            Message = "Test message"
        };
        var mockNotesService = new Mock<INotesService>();
        mockNotesService.Setup(x => x.AddNoteAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()))
            .Verifiable();
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService.Setup(x => x.GetUserAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as UserEntity);
        var controller = new NotesController(mockNotesService.Object, mockUsersService.Object);

        // Act
        var result = await controller.AddNote(note, CancellationToken.None);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        mockNotesService.Verify(x => x.AddNoteAsync(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
    
    [Fact]
    public async Task NotesController_RemoveNote_ReturnsOk()
    {
        // Arrange
        var note = new NoteEntity
        {
            NoteId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            Message = "Test message",
            CreationDate = DateTime.Now
        };
        var mockNotesService = new Mock<INotesService>();
        mockNotesService.Setup(x => x.GetNoteById(It.IsAny<Guid>()))
            .Returns(note);
        mockNotesService.Setup(x => x.RemoveNote(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()))
            .Verifiable();
        var mockUsersService = new Mock<IUsersService>();
        
        var claim = new Claim("oid", note.AuthorId.ToString());
        var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
        var claimsPrincipal = new ClaimsPrincipal(mockIdentity);
        var mockContext = Mock.Of<ControllerContext>(ctx =>
            ctx.HttpContext == Mock.Of<HttpContext>(hCtx => hCtx.User == claimsPrincipal));
        
        var controller = new NotesController(mockNotesService.Object, mockUsersService.Object)
        {
            ControllerContext = mockContext
        };

        // Act
        var result = await controller.RemoveNote(note.NoteId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<OkResult>();
        mockNotesService.Verify(x => x.RemoveNote(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Fact]
    public async Task NotesController_RemoveNote_ReturnsNotFound()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        var mockNotesService = new Mock<INotesService>();
        mockNotesService.Setup(x => x.GetNoteById(It.IsAny<Guid>())).Returns(null as NoteEntity);
        var controller = new NotesController(mockNotesService.Object, mockUsersService.Object);

        // Act
        var result = await controller.RemoveNote(Guid.NewGuid(), CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
    
    [Fact]
    public async Task NotesController_RemoveNote_ReturnsForbid()
    {
        // Arrange
        var note = new NoteEntity
        {
            NoteId = Guid.NewGuid(),
            LessonId = Guid.NewGuid(),
            AuthorId = Guid.NewGuid(),
            Message = "Test message",
            CreationDate = DateTime.Now
        };
        var mockNotesService = new Mock<INotesService>();
        mockNotesService.Setup(x => x.GetNoteById(It.IsAny<Guid>()))
            .Returns(note);
        mockNotesService.Setup(x => x.RemoveNote(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()))
            .Verifiable();
        var mockUsersService = new Mock<IUsersService>();
        
        var claim = new Claim("oid", Guid.NewGuid().ToString());
        var mockIdentity = Mock.Of<ClaimsIdentity>(ci => ci.FindFirst(It.IsAny<string>()) == claim);
        var claimsPrincipal = new ClaimsPrincipal(mockIdentity);
        var mockContext = Mock.Of<ControllerContext>(ctx =>
            ctx.HttpContext == Mock.Of<HttpContext>(hCtx => hCtx.User == claimsPrincipal));
        
        var controller = new NotesController(mockNotesService.Object, mockUsersService.Object)
        {
            ControllerContext = mockContext
        };

        // Act
        var result = await controller.RemoveNote(note.NoteId, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ForbidResult>();
        mockNotesService.Verify(x => x.RemoveNote(It.IsAny<NoteEntity>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}