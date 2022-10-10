using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.Data.LocalMigrations.Migrations
{
    public partial class Adding_Organisation_GST_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_CityTown",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_Street1",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_Street2",
                table: "Organisation");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine1",
                table: "Organisation",
                type: "TEXT",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine2",
                table: "Organisation",
                type: "TEXT",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine3",
                table: "Organisation",
                type: "TEXT",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostCode",
                table: "Organisation",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GSTNumber",
                table: "Organisation",
                type: "TEXT",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Phone_PhoneNumber",
                table: "Organisation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Phone_PhoneNumberPrefix",
                table: "Organisation",
                type: "INTEGER",
                maxLength: 3,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_AddressLine1",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine2",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine3",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address_PostCode",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "GSTNumber",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Phone_PhoneNumber",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Phone_PhoneNumberPrefix",
                table: "Organisation");

            migrationBuilder.AddColumn<string>(
                name: "Address_CityTown",
                table: "Organisation",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Street1",
                table: "Organisation",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Street2",
                table: "Organisation",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
