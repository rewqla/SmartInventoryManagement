using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        string HashPassword(string password)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] salt = RandomNumberGenerator.GetBytes(16);
                byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 500000, HashAlgorithmName.SHA512, 32);

                return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
            }
        }
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminRoleId = Guid.NewGuid();
            var managerRoleId = Guid.NewGuid();
            var workerRoleId = Guid.NewGuid();
            
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{adminRoleId}','Admin')");
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{managerRoleId}','Manager')");
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{workerRoleId}','Worker')");
            
            migrationBuilder.Sql($"INSERT INTO \"Users\" (\"Id\", \"Name\", \"Phone\", \"Email\", \"PasswordHash\", \"RoleId\") VALUES ('{Guid.NewGuid()}', 'Admin User', '1234567890', 'admin@example.com', '{HashPassword("123456")}', '{adminRoleId}')");
            migrationBuilder.Sql($"INSERT INTO \"Users\" (\"Id\", \"Name\", \"Phone\", \"Email\", \"PasswordHash\", \"RoleId\") VALUES ('{Guid.NewGuid()}', 'Manager User', '0987654321', 'manager@example.com', '{HashPassword("123456")}', '{managerRoleId}')");
            migrationBuilder.Sql($"INSERT INTO \"Users\" (\"Id\", \"Name\", \"Phone\", \"Email\", \"PasswordHash\", \"RoleId\") VALUES ('{Guid.NewGuid()}', 'Worker User', '1112223333', 'worker@example.com', '{HashPassword("123456")}', '{workerRoleId}')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
