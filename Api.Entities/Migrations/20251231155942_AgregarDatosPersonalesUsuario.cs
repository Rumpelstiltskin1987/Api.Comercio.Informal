using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Entities.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosPersonalesUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cobrador_Id_cobrador",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Id_cobrador",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id_cobrador",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "A_materno",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "A_paterno",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Fecha_alta",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Usuario_alta",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "A_materno",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "A_paterno",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Fecha_alta",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Usuario_alta",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Id_cobrador",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Id_cobrador",
                table: "AspNetUsers",
                column: "Id_cobrador");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cobrador_Id_cobrador",
                table: "AspNetUsers",
                column: "Id_cobrador",
                principalTable: "Cobrador",
                principalColumn: "Id_cobrador",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
