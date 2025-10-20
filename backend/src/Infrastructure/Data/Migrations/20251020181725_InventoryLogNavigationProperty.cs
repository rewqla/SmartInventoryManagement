using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InventoryLogNavigationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChangedById",
                schema: "public",
                table: "InventoryLogs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_InventoryLogs_ChangedById",
                schema: "public",
                table: "InventoryLogs",
                column: "ChangedById");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryLogs_Users_ChangedById",
                schema: "public",
                table: "InventoryLogs",
                column: "ChangedById",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryLogs_Users_ChangedById",
                schema: "public",
                table: "InventoryLogs");

            migrationBuilder.DropIndex(
                name: "IX_InventoryLogs_ChangedById",
                schema: "public",
                table: "InventoryLogs");

            migrationBuilder.DropColumn(
                name: "ChangedById",
                schema: "public",
                table: "InventoryLogs");
        }
    }
}
