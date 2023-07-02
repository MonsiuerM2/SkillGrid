using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class OrganizationTableUpdatedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "Session",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrganizationOrgId",
                table: "Session",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Organizations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Session_OrganizationOrgId",
                table: "Session",
                column: "OrganizationOrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Organizations_OrganizationOrgId",
                table: "Session",
                column: "OrganizationOrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_Organizations_OrganizationOrgId",
                table: "Session");

            migrationBuilder.DropIndex(
                name: "IX_Session_OrganizationOrgId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "OrganizationOrgId",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Organizations");
        }
    }
}
