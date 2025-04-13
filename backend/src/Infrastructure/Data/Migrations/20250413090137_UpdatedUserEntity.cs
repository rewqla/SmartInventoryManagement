using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                schema: "public",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "Users",
                newName: "FullName");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Phone",
                schema: "public",
                table: "Users",
                newName: "IX_Users_PhoneNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "FailedLoginAttempts",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockoutEnd",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FailedLoginAttempts",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                schema: "public",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                schema: "public",
                table: "Users",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "public",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Users_PhoneNumber",
                schema: "public",
                table: "Users",
                newName: "IX_Users_Phone");
        }
    }
}
