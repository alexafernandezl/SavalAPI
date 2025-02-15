using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SavalAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFormularioEncuestado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "FormularioEncuestados");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FormularioEncuestados",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
