namespace WebTimetable.Application.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Group { get; set; }
    public ICollection<NoteEntity> Notes { get; set; }
    public bool IsRestricted { get; set; }
    public DateTime CreationDate { get; set; }
}