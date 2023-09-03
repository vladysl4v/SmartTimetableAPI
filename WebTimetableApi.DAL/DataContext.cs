using Microsoft.EntityFrameworkCore;

using WebTimetableApi.DAL.Entities;


namespace WebTimetableApi.DAL
{
    public class DataContext : DbContext
    {
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasMany(e => e.Notes)
                .WithOne(e => e.User);
        }
    }
}
