namespace WebTimetable.Application.Entities
{
    public class NoteEntity
    {
        public Guid NoteId { get; set; } = Guid.NewGuid();
        public Guid AuthorId { get; set; }
        public Guid LessonId { get; set; }
        public string Message { get; set; }
        public string AuthorGroup { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    }
}
