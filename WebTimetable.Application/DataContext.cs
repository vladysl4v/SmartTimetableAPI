using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using WebTimetable.Application.Entities;
using WebTimetable.Application.Models;


namespace WebTimetable.Application
{
    public class DataContext : DbContext
    {
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OutageEntity> Outages { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutageEntity>().HasKey(x => new {x.City, x.Group, x.DayOfWeek});
            modelBuilder.Entity<OutageEntity>().Property(x => x.City).HasMaxLength(50);
            modelBuilder.Entity<OutageEntity>().Property(x => x.Group).HasMaxLength(50);
            modelBuilder.Entity<OutageEntity>().Property(x => x.Outages).HasConversion(
                parseIn => JsonConvert.SerializeObject(parseIn),
                parseOut => JsonConvert.DeserializeObject<List<Outage>>(parseOut)!,
                new ValueComparer<List<Outage>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
            
            modelBuilder.Entity<NoteEntity>().HasKey(x => x.NoteId);
            modelBuilder.Entity<NoteEntity>().HasIndex(x => x.LessonId).HasMethod("hash");
            modelBuilder.Entity<NoteEntity>().Property(x => x.Message).HasMaxLength(256);
            modelBuilder.Entity<NoteEntity>().Property(x => x.NoteId).HasDefaultValueSql("gen_random_uuid()");
            modelBuilder.Entity<NoteEntity>().Property(x => x.CreationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<UserEntity>().Property(x => x.Group).HasMaxLength(60);
            modelBuilder.Entity<UserEntity>().Property(x => x.FullName).HasMaxLength(120);
            modelBuilder.Entity<UserEntity>().Property(x => x.CreationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<UserEntity>()
                .HasMany(e => e.Notes)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
