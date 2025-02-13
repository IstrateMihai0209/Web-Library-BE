using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReadingHistories_Users_UserId",
                table: "ReadingHistories");

            migrationBuilder.DropIndex(
                name: "IX_ReadingHistories_UserId",
                table: "ReadingHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ReadingHistories_UserId",
                table: "ReadingHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingHistories_Users_UserId",
                table: "ReadingHistories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
