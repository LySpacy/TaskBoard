using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskBoard.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBanList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("012676f1-5da6-422d-96dc-15e5837a5e8f"));

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "BlackList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateBan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Сause = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlackList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackList_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role" },
                values: new object[] { new Guid("691ff302-6b4d-4007-897a-72559e810e37"), "lyspacywork@gmail.com", "FirstAdmin", "240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9", 2 });

            migrationBuilder.CreateIndex(
                name: "IX_BlackList_UserId",
                table: "BlackList",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackList");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("691ff302-6b4d-4007-897a-72559e810e37"));

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "PasswordHash", "Role" },
                values: new object[] { new Guid("012676f1-5da6-422d-96dc-15e5837a5e8f"), "lyspacywork@gmail.com", "FirstAdmin", "240BE518FABD2724DDB6F04EEB1DA5967448D7E831C08C8FA822809F74C720A9", "Администратор" });
        }
    }
}
