using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacovallet.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsertBooleanValueOnCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSystem",
                schema: "Pacovallet",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSystem",
                schema: "Pacovallet",
                table: "Categories");
        }
    }
}
