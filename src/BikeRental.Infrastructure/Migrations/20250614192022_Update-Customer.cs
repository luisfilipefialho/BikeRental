using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BikeRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CnhImageFileName",
                table: "Customers",
                newName: "CnhImageBase64");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CnhImageBase64",
                table: "Customers",
                newName: "CnhImageFileName");
        }
    }
}
