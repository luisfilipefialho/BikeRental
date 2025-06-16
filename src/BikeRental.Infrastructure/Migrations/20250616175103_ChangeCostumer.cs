using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCostumer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CnhImageBase64",
                table: "Customers",
                newName: "CnhImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CnhImagePath",
                table: "Customers",
                newName: "CnhImageBase64");
        }
    }
}
