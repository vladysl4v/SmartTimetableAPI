namespace WebTimetable.Application.Entities
{
    public class NoteEntity
    {
        public Guid NoteId { get; set; }
        public Guid LessonId { get; set; }
        public string Message { get; set; }
        public Guid AuthorId { get; set; }
        public UserEntity Author { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
