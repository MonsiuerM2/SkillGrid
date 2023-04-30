using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class CourseManagementEntities3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Course_CourseId1",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Organization_OrgId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Course_CourseId1",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Module_ModuleId1",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAssignment_Lecturer_LecturerId",
                table: "ModuleAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAssignment_Module_ModuleId",
                table: "ModuleAssignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_Course_CourseId",
                table: "Session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organization",
                table: "Organization");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModuleAssignment",
                table: "ModuleAssignment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Module",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecturer",
                table: "Lecturer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Course",
                table: "Course");

            migrationBuilder.RenameTable(
                name: "Organization",
                newName: "Organizations");

            migrationBuilder.RenameTable(
                name: "ModuleAssignment",
                newName: "ModuleAssignments");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "Lecturer",
                newName: "Lecturers");

            migrationBuilder.RenameTable(
                name: "Course",
                newName: "Courses");

            migrationBuilder.RenameIndex(
                name: "IX_ModuleAssignment_ModuleId",
                table: "ModuleAssignments",
                newName: "IX_ModuleAssignments_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ModuleAssignment_LecturerId",
                table: "ModuleAssignments",
                newName: "IX_ModuleAssignments_LecturerId");

            migrationBuilder.RenameIndex(
                name: "IX_Module_ModuleId1",
                table: "Modules",
                newName: "IX_Modules_ModuleId1");

            migrationBuilder.RenameIndex(
                name: "IX_Module_CourseId1",
                table: "Modules",
                newName: "IX_Modules_CourseId1");

            migrationBuilder.RenameIndex(
                name: "IX_Module_CourseId",
                table: "Modules",
                newName: "IX_Modules_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Course_OrgId",
                table: "Courses",
                newName: "IX_Courses_OrgId");

            migrationBuilder.RenameIndex(
                name: "IX_Course_CourseId1",
                table: "Courses",
                newName: "IX_Courses_CourseId1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Modules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations",
                column: "OrgId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModuleAssignments",
                table: "ModuleAssignments",
                column: "AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecturers",
                table: "Lecturers",
                column: "LecturerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Courses",
                table: "Courses",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_CourseId1",
                table: "Courses",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Organizations_OrgId",
                table: "Courses",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAssignments_Lecturers_LecturerId",
                table: "ModuleAssignments",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAssignments_Modules_ModuleId",
                table: "ModuleAssignments",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Courses_CourseId1",
                table: "Modules",
                column: "CourseId1",
                principalTable: "Courses",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Modules_ModuleId1",
                table: "Modules",
                column: "ModuleId1",
                principalTable: "Modules",
                principalColumn: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Courses_CourseId",
                table: "Session",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_CourseId1",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Organizations_OrgId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAssignments_Lecturers_LecturerId",
                table: "ModuleAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAssignments_Modules_ModuleId",
                table: "ModuleAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Courses_CourseId1",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Modules_ModuleId1",
                table: "Modules");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_Courses_CourseId",
                table: "Session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Organizations",
                table: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ModuleAssignments",
                table: "ModuleAssignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecturers",
                table: "Lecturers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Courses",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "Organizations",
                newName: "Organization");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Module");

            migrationBuilder.RenameTable(
                name: "ModuleAssignments",
                newName: "ModuleAssignment");

            migrationBuilder.RenameTable(
                name: "Lecturers",
                newName: "Lecturer");

            migrationBuilder.RenameTable(
                name: "Courses",
                newName: "Course");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_ModuleId1",
                table: "Module",
                newName: "IX_Module_ModuleId1");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_CourseId1",
                table: "Module",
                newName: "IX_Module_CourseId1");

            migrationBuilder.RenameIndex(
                name: "IX_Modules_CourseId",
                table: "Module",
                newName: "IX_Module_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_ModuleAssignments_ModuleId",
                table: "ModuleAssignment",
                newName: "IX_ModuleAssignment_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ModuleAssignments_LecturerId",
                table: "ModuleAssignment",
                newName: "IX_ModuleAssignment_LecturerId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_OrgId",
                table: "Course",
                newName: "IX_Course_OrgId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_CourseId1",
                table: "Course",
                newName: "IX_Course_CourseId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Organization",
                table: "Organization",
                column: "OrgId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Module",
                table: "Module",
                column: "ModuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ModuleAssignment",
                table: "ModuleAssignment",
                column: "AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecturer",
                table: "Lecturer",
                column: "LecturerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Course",
                table: "Course",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Course_CourseId1",
                table: "Course",
                column: "CourseId1",
                principalTable: "Course",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Organization_OrgId",
                table: "Course",
                column: "OrgId",
                principalTable: "Organization",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId",
                table: "Module",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Course_CourseId1",
                table: "Module",
                column: "CourseId1",
                principalTable: "Course",
                principalColumn: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Module_ModuleId1",
                table: "Module",
                column: "ModuleId1",
                principalTable: "Module",
                principalColumn: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAssignment_Lecturer_LecturerId",
                table: "ModuleAssignment",
                column: "LecturerId",
                principalTable: "Lecturer",
                principalColumn: "LecturerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAssignment_Module_ModuleId",
                table: "ModuleAssignment",
                column: "ModuleId",
                principalTable: "Module",
                principalColumn: "ModuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Course_CourseId",
                table: "Session",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
