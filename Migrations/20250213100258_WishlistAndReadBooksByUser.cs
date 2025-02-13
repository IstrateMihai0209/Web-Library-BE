using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineLibrary.Migrations
{
    /// <inheritdoc />
    public partial class WishlistAndReadBooksByUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReadBooksModelId",
                table: "Books",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WishlistModelId",
                table: "Books",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReadBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReadBooksModelId",
                table: "Books",
                column: "ReadBooksModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_WishlistModelId",
                table: "Books",
                column: "WishlistModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ReadBooks_ReadBooksModelId",
                table: "Books",
                column: "ReadBooksModelId",
                principalTable: "ReadBooks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Wishlists_WishlistModelId",
                table: "Books",
                column: "WishlistModelId",
                principalTable: "Wishlists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_ReadBooks_ReadBooksModelId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Wishlists_WishlistModelId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "ReadBooks");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Books_ReadBooksModelId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_WishlistModelId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReadBooksModelId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "WishlistModelId",
                table: "Books");
        }
    }
}
