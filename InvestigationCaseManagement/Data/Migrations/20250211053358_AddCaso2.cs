using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestigationCaseManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCaso2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Casos",
                newName: "TipoIrregularidad");

            migrationBuilder.AddColumn<string>(
                name: "Actuaciones",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AreaApoyo",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Conclusiones",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DetallesFraude",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DuracionDias",
                table: "Casos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Incidencia",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModusOperandi",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MovilAfectado",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjetivoAgraviado",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrigenCaso",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Recomendaciones",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Soporte",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubtipoIrregularidad",
                table: "Casos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Actuaciones",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "AreaApoyo",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "Conclusiones",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "DetallesFraude",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "DuracionDias",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "Incidencia",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "ModusOperandi",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "MovilAfectado",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "ObjetivoAgraviado",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "OrigenCaso",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "Recomendaciones",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "Soporte",
                table: "Casos");

            migrationBuilder.DropColumn(
                name: "SubtipoIrregularidad",
                table: "Casos");

            migrationBuilder.RenameColumn(
                name: "TipoIrregularidad",
                table: "Casos",
                newName: "Descripcion");
        }
    }
}
