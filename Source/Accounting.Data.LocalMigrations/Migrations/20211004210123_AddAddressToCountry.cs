using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.Data.LocalMigrations.Migrations
{
    public partial class AddAddressToCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Organisation",
                type: "TEXT",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrganisationId",
                table: "Invoice",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "Customer",
                type: "TEXT",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "OrganisationId",
                table: "Invoice",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Organisation_OrganisationId",
                table: "Invoice",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
