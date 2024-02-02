using Microsoft.EntityFrameworkCore;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Repositories.Abstractions;

namespace WebTimetable.Application.Repositories;

public class NotesRepository : INotesRepository
{
    private readonly DataContext _context;
    
    public NotesRepository(DataContext context)
    {
        _context = context;
    }
    
    public List<NoteEntity> GetNotesByLessonId(Guid lessonId, string userGroup)
    {
        return _context.Notes.AsNoTracking().Include(x => x.Author).Where(entity =>
            entity.LessonId == lessonId && entity.Author.Group == userGroup).ToList();
    }

    public bool IsNoteExists(Guid authorId, Guid lessonId)
    {
        return _context.Notes.AsNoTracking().Any(x => x.Author.Id == authorId && x.LessonId == lessonId);
    }

    public NoteEntity? GetNoteById(Guid noteId)
    {
        return _context.Notes.AsNoTracking().FirstOrDefault(x => x.NoteId == noteId);
    }

    public async Task AddAsync(NoteEntity note, CancellationToken token)
    { 
        await _context.Notes.AddAsync(note, token);
        await _context.SaveChangesAsync(token);
    }

    public async Task RemoveAsync(NoteEntity note, CancellationToken token)
    {
        _context.Remove(note);
        await _context.SaveChangesAsync(token);
    }
}