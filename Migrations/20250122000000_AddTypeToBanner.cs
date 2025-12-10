using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITI_Pharmacy_API.Migrations
{
    /// <inheritdoc />
    public partial class AddTypeToBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Banners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Banners");
        }
    }
}

