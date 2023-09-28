using Microsoft.EntityFrameworkCore;

using WebTimetable.Application.Entities;


namespace WebTimetable.Application
{
    public class DataContext : DbContext
    {
        public DbSet<NoteEntity> Notes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NoteEntity>().HasKey(x => x.NoteId);
            modelBuilder.Entity<NoteEntity>().HasIndex(x => x.LessonId).HasMethod("hash");
            modelBuilder.Entity<NoteEntity>().Property(x => x.AuthorGroup).HasMaxLength(40);
            modelBuilder.Entity<NoteEntity>().Property(x => x.AuthorName).HasMaxLength(60);
        }
    }
}
