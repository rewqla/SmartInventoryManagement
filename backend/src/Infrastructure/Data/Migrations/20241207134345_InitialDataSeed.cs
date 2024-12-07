using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{Guid.NewGuid()}','Admin')");
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{Guid.NewGuid()}','Manager')");
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{Guid.NewGuid()}','Worker')");
            
            // migrationBuilder.InsertData(
            //     table: "Roles",
            //     columns: new[] { "Id", "Name" },
            //     values: new object[,]
            //     {
            //         { Guid.NewGuid(), "Admin" },
            //         { Guid.NewGuid(), "Manager" },
            //         { Guid.NewGuid(), "User" }
            //     });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
