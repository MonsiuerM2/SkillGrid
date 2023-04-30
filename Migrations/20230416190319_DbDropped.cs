using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class DbDropped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Registration_RegistrationRegId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAssignments_Lecturers_LecturerId",
                table: "ModuleAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ModuleAssignments_LecturerId",
                table: "ModuleAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ModuleAssignments_ModuleId",
                table: "ModuleAssignments");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RegistrationRegId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegistrationRegId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Registration",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Registration_StudentId",
                table: "Registration",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleAssignments_LecturerId",
                table: "ModuleAssignments",
                column: "LecturerId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleAssignments_ModuleId",
                table: "ModuleAssignments",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAssignments_AspNetUsers_LecturerId",
                table: "ModuleAssignments",
                column: "LecturerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registration_AspNetUsers_StudentId",
                table: "Registration",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModuleAssignments_AspNetUsers_LecturerId",
                table: "ModuleAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_Registration_AspNetUsers_StudentId",
                table: "Registration");

            migrationBuilder.DropIndex(
                name: "IX_Registration_StudentId",
                table: "Registration");

            migrationBuilder.DropIndex(
                name: "IX_ModuleAssignments_LecturerId",
                table: "ModuleAssignments");

            migrationBuilder.DropIndex(
                name: "IX_ModuleAssignments_ModuleId",
                table: "ModuleAssignments");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Registration");

            migrationBuilder.AddColumn<int>(
                name: "RegistrationRegId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleAssignments_LecturerId",
                table: "ModuleAssignments",
                column: "LecturerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleAssignments_ModuleId",
                table: "ModuleAssignments",
                column: "ModuleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RegistrationRegId",
                table: "AspNetUsers",
                column: "RegistrationRegId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Registration_RegistrationRegId",
                table: "AspNetUsers",
                column: "RegistrationRegId",
                principalTable: "Registration",
                principalColumn: "RegId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleAssignments_Lecturers_LecturerId",
                table: "ModuleAssignments",
                column: "LecturerId",
                principalTable: "Lecturers",
                principalColumn: "LecturerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
