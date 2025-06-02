using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Added_Usernames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "RegisterModel",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "RegisterModel");
        }
    }
}
