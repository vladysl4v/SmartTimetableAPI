﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebTimetable.Application;

#nullable disable

namespace WebTimetable.Application.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231125144933_Add_Outages")]
    partial class Add_Outages
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebTimetable.Application.Entities.NoteEntity", b =>
                {
                    b.Property<Guid>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("LessonId")
                        .HasColumnType("uuid");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("NoteId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("LessonId");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("LessonId"), "hash");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("WebTimetable.Application.Entities.OutageEntity", b =>
                {
                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Group")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer");

                    b.Property<string>("Outages")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("City", "Group", "DayOfWeek");

                    b.ToTable("Outages");
                });

            modelBuilder.Entity("WebTimetable.Application.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("character varying(120)");

                    b.Property<string>("Group")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<bool>("IsRestricted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebTimetable.Application.Entities.NoteEntity", b =>
                {
                    b.HasOne("WebTimetable.Application.Entities.UserEntity", "Author")
                        .WithMany("Notes")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("WebTimetable.Application.Entities.UserEntity", b =>
                {
                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
