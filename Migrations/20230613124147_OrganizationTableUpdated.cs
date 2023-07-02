using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationTableUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Organizations_OrganizationOrgId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_OrganizationOrgId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "OrganizationOrgId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Organizations",
                newName: "WorkType");

            migrationBuilder.AddColumn<string>(
                name: "OrgName",
                table: "Organizations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Services",
                table: "Organizations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
                table: "Organizations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "WorkType",
                table: "Organizations",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "OrganizationOrgId",
                table: "Courses",
                type: "INTEGER",
                nullable: true);

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
        }
    }
}
