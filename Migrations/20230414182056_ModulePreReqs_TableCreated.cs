using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMedRazor.Migrations
{
    /// <inheritdoc />
    public partial class ModulePreReqsTableCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModulePreReqs",
                columns: table => new
                {
                    ModulePreReqsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    PreReqId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePreReqs", x => x.ModulePreReqsId);
                    table.ForeignKey(
                        name: "FK_ModulePreReqs_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModulePreReqs_Modules_PreReqId",
                        column: x => x.PreReqId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModulePreReqs_ModuleId",
                table: "ModulePreReqs",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModulePreReqs_PreReqId",
                table: "ModulePreReqs",
                column: "PreReqId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModulePreReqs");
        }
    }
}
