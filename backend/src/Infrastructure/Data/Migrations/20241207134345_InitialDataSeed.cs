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
            SeedRoles(migrationBuilder);
            SeedUsers(migrationBuilder);
            SeedCategories(migrationBuilder);
            SeedWarehouses(migrationBuilder);
        }

        private Guid _adminRoleId;
        private Guid _managerRoleId;
        private Guid _workerRoleId;
        
        private void SeedRoles(MigrationBuilder migrationBuilder)
        {
             _adminRoleId = Guid.NewGuid();
             _managerRoleId = Guid.NewGuid();
             _workerRoleId = Guid.NewGuid();

            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{_adminRoleId}','Admin')");
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{_managerRoleId}','Manager')");
            migrationBuilder.Sql($"INSERT INTO \"Roles\" (\"Id\", \"Name\") VALUES ('{_workerRoleId}','Worker')");
        }
        
        private void SeedUsers(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO \"Users\" (\"Id\", \"Name\", \"Phone\", \"Email\", \"PasswordHash\", \"RoleId\") VALUES ('{Guid.NewGuid()}', 'Admin User', '1234567890', 'admin@example.com', '{HashPassword("123456")}', '{_adminRoleId}')");
            migrationBuilder.Sql($"INSERT INTO \"Users\" (\"Id\", \"Name\", \"Phone\", \"Email\", \"PasswordHash\", \"RoleId\") VALUES ('{Guid.NewGuid()}', 'Manager User', '0987654321', 'manager@example.com', '{HashPassword("123456")}', '{_managerRoleId}')");
            migrationBuilder.Sql($"INSERT INTO \"Users\" (\"Id\", \"Name\", \"Phone\", \"Email\", \"PasswordHash\", \"RoleId\") VALUES ('{Guid.NewGuid()}', 'Worker User', '1112223333', 'worker@example.com', '{HashPassword("123456")}', '{_workerRoleId}')");
        }
        
        private Guid _electronicsCategoryId;
        private Guid _furnitureCategoryId;
        private Guid _clothingCategoryId;
        private Guid _sportsCategoryId;
        private Guid _beautyCategoryId;
        private void SeedCategories(MigrationBuilder migrationBuilder)
        {
            _electronicsCategoryId = Guid.NewGuid();
            _furnitureCategoryId = Guid.NewGuid();
            _clothingCategoryId = Guid.NewGuid();
            _sportsCategoryId = Guid.NewGuid();
            _beautyCategoryId = Guid.NewGuid();

            migrationBuilder.Sql($"INSERT INTO \"Categories\" (\"Id\", \"Name\") VALUES ('{_electronicsCategoryId}', 'Electronics')");
            migrationBuilder.Sql($"INSERT INTO \"Categories\" (\"Id\", \"Name\") VALUES ('{_furnitureCategoryId}', 'Furniture')");
            migrationBuilder.Sql($"INSERT INTO \"Categories\" (\"Id\", \"Name\") VALUES ('{_clothingCategoryId}', 'Clothing')");
            migrationBuilder.Sql($"INSERT INTO \"Categories\" (\"Id\", \"Name\") VALUES ('{_sportsCategoryId}', 'Sports')");
            migrationBuilder.Sql($"INSERT INTO \"Categories\" (\"Id\", \"Name\") VALUES ('{_beautyCategoryId}', 'Beauty')");
        }
        private Guid _warehouse1Id;
        private Guid _warehouse2Id;
        private void SeedWarehouses(MigrationBuilder migrationBuilder)
        {
            _warehouse1Id = Guid.NewGuid();
            _warehouse2Id = Guid.NewGuid();

            migrationBuilder.Sql($"INSERT INTO \"Warehouses\" (\"Id\", \"Name\", \"Location\") VALUES ('{_warehouse1Id}', 'Central Warehouse','Rivne')");
            migrationBuilder.Sql($"INSERT INTO \"Warehouses\" (\"Id\", \"Name\", \"Location\") VALUES ('{_warehouse2Id}', 'Secondary Warehouse', 'Zhytomyr')");
        }
        
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
