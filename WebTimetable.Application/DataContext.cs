using Microsoft.EntityFrameworkCore;

using WebTimetable.Application.Entities;


namespace WebTimetable.Application
{
    public class DataContext : DbContext
    {
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NoteEntity>().HasKey(x => x.NoteId);
            modelBuilder.Entity<NoteEntity>().HasIndex(x => x.LessonId).HasMethod("hash");
            modelBuilder.Entity<NoteEntity>().Property(x => x.Message).HasMaxLength(256);

            modelBuilder.Entity<UserEntity>().Property(x => x.Group).HasMaxLength(60);
            modelBuilder.Entity<UserEntity>().Property(x => x.FullName).HasMaxLength(120);

            modelBuilder.Entity<UserEntity>().HasMany(e => e.Notes).WithOne(e => e.Author).HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
