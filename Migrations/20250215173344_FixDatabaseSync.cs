using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavalAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixDatabaseSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Verificar si la columna Altura existe antes de eliminarla
            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Encuestados' AND COLUMN_NAME = 'Altura') " +
                                 "BEGIN ALTER TABLE Encuestados DROP COLUMN Altura END;");

            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Encuestados' AND COLUMN_NAME = 'Peso') " +
                                 "BEGIN ALTER TABLE Encuestados DROP COLUMN Peso END;");

            migrationBuilder.Sql("IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Encuestados' AND COLUMN_NAME = 'Ubicacion') " +
                                 "BEGIN ALTER TABLE Encuestados DROP COLUMN Ubicacion END;");

            migrationBuilder.AddColumn<bool>(
                name: "Habilitado",
                table: "Encuestados",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FormularioEncuestados",
                columns: table => new
                {
                    IdFormulario = table.Column<int>(type: "int", nullable: false),
                    IdEncuestado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Altura = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Peso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IMC = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormularioEncuestados", x => new { x.IdFormulario, x.IdEncuestado, x.Fecha });
                    table.ForeignKey(
                        name: "FK_FormularioEncuestados_Encuestados_IdEncuestado",
                        column: x => x.IdEncuestado,
                        principalTable: "Encuestados",
                        principalColumn: "Identificacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormularioEncuestados_Formularios_IdFormulario",
                        column: x => x.IdFormulario,
                        principalTable: "Formularios",
                        principalColumn: "IdFormulario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormularioEncuestados_IdEncuestado",
                table: "FormularioEncuestados",
                column: "IdEncuestado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormularioEncuestados");

            migrationBuilder.DropColumn(
                name: "Habilitado",
                table: "Encuestados");

            migrationBuilder.AddColumn<decimal>(
                name: "Altura",
                table: "Encuestados",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Peso",
                table: "Encuestados",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Ubicacion",
                table: "Encuestados",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
