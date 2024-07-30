using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolManagement.Migrations
{
    public partial class initLoad2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subject_SubjectsId",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Subject_SubjectsId",
                table: "SubjectTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subjects_SubjectsId",
                table: "StudentSubject",
                column: "SubjectsId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Subjects_SubjectsId",
                table: "SubjectTeacher",
                column: "SubjectsId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubject_Subjects_SubjectsId",
                table: "StudentSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Subjects_SubjectsId",
                table: "SubjectTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubject_Subject_SubjectsId",
                table: "StudentSubject",
                column: "SubjectsId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Subject_SubjectsId",
                table: "SubjectTeacher",
                column: "SubjectsId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
