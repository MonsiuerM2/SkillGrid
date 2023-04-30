using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class CourseManagementEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_Course_SessionId",
                table: "Session");

            migrationBuilder.CreateIndex(
                name: "IX_Session_CourseId",
                table: "Session",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Course_CourseId",
                table: "Session",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_Course_CourseId",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_Session_CourseId",
                table: "Session");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Course_SessionId",
                table: "Session",
                column: "SessionId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
