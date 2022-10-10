using Microsoft.EntityFrameworkCore.Migrations;

namespace Accounting.Data.LocalMigrations.Migrations
{
    public partial class AddUserPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organisation__organisationId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Phone_PhoneNumberPrefix",
                table: "Organisation",
                type: "INTEGER",
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

            migrationBuilder.AddColumn<int>(
                name: "Phone_PhoneNumber",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Phone_PhoneNumberPrefix",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organisation__organisationId",
                table: "AspNetUsers",
                column: "_organisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Organisation__organisationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone_PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone_PhoneNumberPrefix",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Phone_PhoneNumberPrefix",
                table: "Organisation",
                type: "INTEGER",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "Phone_PhoneNumber",
                table: "Organisation",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Organisation__organisationId",
                table: "AspNetUsers",
                column: "_organisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
