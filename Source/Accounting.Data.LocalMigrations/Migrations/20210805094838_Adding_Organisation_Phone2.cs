using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.Data.LocalMigrations.Migrations
{
    public partial class Adding_Organisation_Phone2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Phone_PhoneNumberPrefix",
                table: "Organisation",
                type: "INTEGER",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<int>(
                name: "Phone_PhoneNumber",
                table: "Organisation",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Phone_PhoneNumberPrefix",
                table: "Organisation",
                type: "INTEGER",
                maxLength: 3,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldMaxLength: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Phone_PhoneNumber",
                table: "Organisation",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
