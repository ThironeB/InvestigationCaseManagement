using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestigationCaseManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubtipoIrregularidad",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "TipoIrregularidad",
                table: "Casos");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProcedenciaCasoId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProcesoCorregidoId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProcesoRealizadoId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubTipoFichaId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubTipoIrregularidadId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoBrechaId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoIrregularidadId",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "ProcedenciaCasoId",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "ProcesoCorregidoId",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "ProcesoRealizadoId",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "SubTipoFichaId",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "SubTipoIrregularidadId",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "TipoBrechaId",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "TipoIrregularidadId",
                table: "Casos");

            migrationBuilder.AddColumn<string>(
                name: "SubtipoIrregularidad",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TipoIrregularidad",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
