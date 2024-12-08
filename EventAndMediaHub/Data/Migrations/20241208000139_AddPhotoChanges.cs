using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventAndMediaHub.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Photos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Photos");
        }
    }
}
