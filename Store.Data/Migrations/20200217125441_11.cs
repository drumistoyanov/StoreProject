using Microsoft.EntityFrameworkCore.Migrations;

namespace Store.Data.Migrations
{
    public partial class _11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bulsta",
                table: "Bills",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bulstat",
                table: "Bills",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bulsta",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "Bulstat",
                table: "Bills");
        }
    }
}
