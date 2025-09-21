using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace avatCo.Migrations
{
    /// <inheritdoc />
    public partial class AddProductFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialOffer",
                table: "Products",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSpecialOffer",
                table: "Products");
        }
    }
}
