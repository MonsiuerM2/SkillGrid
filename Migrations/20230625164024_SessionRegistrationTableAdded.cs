using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class SessionRegistrationTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Session_Courses_CourseId",
                table: "Session");

            migrationBuilder.DropForeignKey(
                name: "FK_Session_Organizations_OrganizationOrgId",
                table: "Session");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Session",
                table: "Session");

            migrationBuilder.RenameTable(
                name: "Session",
                newName: "Sessions");

            migrationBuilder.RenameIndex(
                name: "IX_Session_OrganizationOrgId",
                table: "Sessions",
                newName: "IX_Sessions_OrganizationOrgId");

            migrationBuilder.RenameIndex(
                name: "IX_Session_CourseId",
                table: "Sessions",
                newName: "IX_Sessions_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions",
                column: "SessionId");

            migrationBuilder.CreateTable(
                name: "SessionRegistrations",
                columns: table => new
                {
                    SessionRegId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateRegistered = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRegistrations", x => x.SessionRegId);
                    table.ForeignKey(
                        name: "FK_SessionRegistrations_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionRegistrations_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionRegistrations_SessionId",
                table: "SessionRegistrations",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRegistrations_StudentId",
                table: "SessionRegistrations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Courses_CourseId",
                table: "Sessions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Organizations_OrganizationOrgId",
                table: "Sessions",
                column: "OrganizationOrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Courses_CourseId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Organizations_OrganizationOrgId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "SessionRegistrations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sessions",
                table: "Sessions");

            migrationBuilder.RenameTable(
                name: "Sessions",
                newName: "Session");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_OrganizationOrgId",
                table: "Session",
                newName: "IX_Session_OrganizationOrgId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_CourseId",
                table: "Session",
                newName: "IX_Session_CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Session",
                table: "Session",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Courses_CourseId",
                table: "Session",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Session_Organizations_OrganizationOrgId",
                table: "Session",
                column: "OrganizationOrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
