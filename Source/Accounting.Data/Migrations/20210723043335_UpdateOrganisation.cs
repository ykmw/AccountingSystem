using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.Data.Migrations
{
    public partial class UpdateOrganisation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice");

            migrationBuilder.DropTable(
                name: "UserOrganisation");

            migrationBuilder.AddColumn<string>(
                name: "Address_CityTown",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Street1",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Street2",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortCode",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrganisationId",
                table: "Invoice",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "_organisationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers__organisationId",
                table: "AspNetUsers",
                column: "_organisationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organisation__organisationId",
                table: "AspNetUsers",
                column: "_organisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organisation__organisationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers__organisationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address_CityTown",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_Street1",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_Street2",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "ShortCode",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "_organisationId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "OrganisationId",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserOrganisation",
                columns: table => new
                {
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganisation", x => new { x.OrganisationId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserOrganisation_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrganisation_Organisation_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganisation_UsersId",
                table: "UserOrganisation",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
