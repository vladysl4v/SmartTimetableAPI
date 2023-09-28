using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebTimetable.Application.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    AuthorGroup = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    AuthorName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_LessonId",
                table: "Notes",
                column: "LessonId")
                .Annotation("Npgsql:IndexMethod", "hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notes");
        }
    }
}
