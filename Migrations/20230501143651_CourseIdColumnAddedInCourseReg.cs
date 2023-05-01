using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class CourseIdColumnAddedInCourseReg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LectureId",
                table: "CourseRegistrations",
                newName: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseRegistrations",
                newName: "LectureId");
        }
    }
}
