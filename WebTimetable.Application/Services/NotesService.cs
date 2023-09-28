using System.Security.Cryptography;
using System.Text;

using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;
using WebTimetable.Application.Repositories;
using WebTimetable.Application.Services.Abstractions;


namespace WebTimetable.Application.Services;

public class NotesService : INotesService
{
    private readonly IDbRepository _dbRepository;
    public NotesService(IDbRepository dbRepository)
    {
        _dbRepository = dbRepository;
    }

    public async Task AddNoteAsync(NoteEntity note)
    {
        await _dbRepository.Add(note);
        await _dbRepository.SaveChangesAsync();
    }

    public List<NoteEntity> GetNotesByLessonId(Guid lessonId, string group)
    {
        return _dbRepository.Get<NoteEntity>(x => x.LessonId == lessonId && x.AuthorGroup == group).ToList();
    }
}