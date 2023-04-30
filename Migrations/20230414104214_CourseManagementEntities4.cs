using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class CourseManagementEntities4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Modules_ModuleId1",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_ModuleId1",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ModuleId1",
                table: "Modules");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModuleId1",
                table: "Modules",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ModuleId1",
                table: "Modules",
                column: "ModuleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Modules_ModuleId1",
                table: "Modules",
                column: "ModuleId1",
                principalTable: "Modules",
                principalColumn: "ModuleId");
        }
    }
}
