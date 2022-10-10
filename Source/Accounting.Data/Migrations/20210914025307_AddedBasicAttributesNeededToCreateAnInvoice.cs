using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.Data.Migrations
{
    public partial class AddedBasicAttributesNeededToCreateAnInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Invoice",
                newName: "PurchaseOrder");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "LineItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GST",
                table: "LineItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsZeroRated",
                table: "LineItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "LineItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "LineItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Discount",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GST",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsGSTExclusive",
                table: "Invoice",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Invoice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine1",
                table: "Customer",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine2",
                table: "Customer",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine3",
                table: "Customer",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostCode",
                table: "Customer",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "Customer",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "Customer",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsGSTExempt",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Phone_PhoneNumber",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone_PhoneNumberPrefix",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "GST",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "IsZeroRated",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "LineItem");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "GST",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "IsGSTExclusive",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine1",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine2",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine3",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Address_PostCode",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IsGSTExempt",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Phone_PhoneNumber",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Phone_PhoneNumberPrefix",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrder",
                table: "Invoice",
                newName: "Name");
        }
    }
}
