namespace WebTimetableApi.DAL.Entities
{
    public class NoteEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public UserEntity User { get; set; }
        public string Text { get; set; }
        public long LessonHashCode { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    }
}
