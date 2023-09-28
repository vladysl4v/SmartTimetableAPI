﻿using Microsoft.EntityFrameworkCore;

using WebTimetable.Application.Entities;


namespace WebTimetable.Application
{
    public class DataContext : DbContext
    {
        public DbSet<NoteEntity> Notes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
            modelBuilder.Entity<NoteEntity>().HasIndex(x => x.LessonId).HasMethod("hash");
                .WithOne(e => e.User);
        }
    }
}