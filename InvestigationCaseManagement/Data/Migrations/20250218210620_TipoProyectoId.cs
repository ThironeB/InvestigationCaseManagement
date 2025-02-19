using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestigationCaseManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class TipoProyectoId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoProyectoId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoProyectoId",
                table: "Casos");
        }
    }
}
