namespace WebTimetable.Application.Entities
{
    public class UserEntity
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string StudyGroupId { get; set; }
        public int OutagesGroup { get; set; }
        public ICollection<NoteEntity> Notes { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
    }
}
