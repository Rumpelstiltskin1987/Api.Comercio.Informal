using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporalPasswordField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsPasswordTemporal",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsPasswordTemporal",
                table: "AspNetUsers");
        }
    }
}
