using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrailDBOUserDBO",
                columns: table => new
                {
                    FavoriteTrailsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserDBOId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailDBOUserDBO", x => new { x.FavoriteTrailsId, x.UserDBOId });
                    table.ForeignKey(
                        name: "FK_TrailDBOUserDBO_Trails_FavoriteTrailsId",
                        column: x => x.FavoriteTrailsId,
                        principalTable: "Trails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrailDBOUserDBO_Users_UserDBOId",
                        column: x => x.UserDBOId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TrailDBOUserDBO_UserDBOId",
                table: "TrailDBOUserDBO",
                column: "UserDBOId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrailDBOUserDBO");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
