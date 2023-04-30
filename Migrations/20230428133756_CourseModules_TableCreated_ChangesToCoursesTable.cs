using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class CourseModulesTableCreatedChangesToCoursesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Organizations_OrgId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Lecturers");

            migrationBuilder.DropIndex(
                name: "IX_Courses_OrgId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Modules",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationOrgId",
                table: "Courses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseId",
                table: "Modules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OrganizationOrgId",
                table: "Courses",
                column: "OrganizationOrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Organizations_OrganizationOrgId",
                table: "Courses",
                column: "OrganizationOrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Organizations_OrganizationOrgId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_CourseId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Courses_OrganizationOrgId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OrganizationOrgId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lecturers",
                columns: table => new
                {
                    LecturerId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecturers", x => x.LecturerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_OrgId",
                table: "Courses",
                column: "OrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Organizations_OrgId",
                table: "Courses",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
