using System.Globalization;
using System.Security.Cryptography;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
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
            SeedProducts(migrationBuilder);
            SeedInventories(migrationBuilder);
            SeedInventoryLogs(migrationBuilder);
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
        
        private Guid _electronicsProductId;
        private Guid _furnitureProductId;
        private Guid _clothingProductId;
        private Guid _sportsProductId;
        private Guid _beautyProductId;
        private void SeedProducts(MigrationBuilder migrationBuilder)
        {
            _electronicsProductId = Guid.NewGuid();
            _furnitureProductId = Guid.NewGuid();
            _clothingProductId = Guid.NewGuid();
            _sportsProductId = Guid.NewGuid();
            _beautyProductId = Guid.NewGuid();

            var products = new[]
            {
                new { Id = _electronicsProductId, Name = "Smartphone", SKU = "ELEC-001", Description = "Latest smartphone", UnitPrice = 699.99, CategoryId = _electronicsCategoryId },
                new { Id = Guid.NewGuid(), Name = "Laptop", SKU = "ELEC-002", Description = "High-performance laptop", UnitPrice = 1299.99, CategoryId = _electronicsCategoryId },
                new { Id = _furnitureProductId, Name = "Office Chair", SKU = "FURN-001", Description = "Ergonomic office chair", UnitPrice = 149.99, CategoryId = _furnitureCategoryId },
                new { Id = Guid.NewGuid(), Name = "Sofa", SKU = "FURN-002", Description = "Comfortable sofa", UnitPrice = 499.99, CategoryId = _furnitureCategoryId },
                new { Id = _clothingProductId, Name = "T-Shirt", SKU = "CLOT-001", Description = "100% cotton T-shirt", UnitPrice = 19.99, CategoryId = _clothingCategoryId },
                new { Id = Guid.NewGuid(), Name = "Jeans", SKU = "CLOT-002", Description = "Stylish jeans", UnitPrice = 49.99, CategoryId = _clothingCategoryId },
                new { Id =_sportsProductId, Name = "Football", SKU = "SPRT-001", Description = "Professional football", UnitPrice = 29.99, CategoryId = _sportsCategoryId },
                new { Id = Guid.NewGuid(), Name = "Basketball", SKU = "SPRT-002", Description = "Durable basketball", UnitPrice = 39.99, CategoryId = _sportsCategoryId },
                new { Id = Guid.NewGuid(), Name = "Lipstick", SKU = "BEAU-001", Description = "Matte lipstick", UnitPrice = 9.99, CategoryId = _beautyCategoryId },
                new { Id = _beautyProductId, Name = "Perfume", SKU = "BEAU-002", Description = "Luxury perfume", UnitPrice = 99.99, CategoryId = _beautyCategoryId },
                new { Id = Guid.NewGuid(), Name = "Tablet", SKU = "ELEC-003", Description = "Portable tablet", UnitPrice = 499.99, CategoryId = _electronicsCategoryId },
                new { Id = Guid.NewGuid(), Name = "Headphones", SKU = "ELEC-004", Description = "Noise-cancelling headphones", UnitPrice = 199.99, CategoryId = _electronicsCategoryId },
                new { Id = Guid.NewGuid(), Name = "Desk", SKU = "FURN-003", Description = "Wooden office desk", UnitPrice = 299.99, CategoryId = _furnitureCategoryId },
                new { Id = Guid.NewGuid(), Name = "Bookshelf", SKU = "FURN-004", Description = "5-shelf bookshelf", UnitPrice = 129.99, CategoryId = _furnitureCategoryId },
                new { Id = Guid.NewGuid(), Name = "Jacket", SKU = "CLOT-003", Description = "Waterproof jacket", UnitPrice = 89.99, CategoryId = _clothingCategoryId },
                new { Id = Guid.NewGuid(), Name = "Sneakers", SKU = "CLOT-004", Description = "Comfortable sneakers", UnitPrice = 79.99, CategoryId = _clothingCategoryId },
                new { Id = Guid.NewGuid(), Name = "Tennis Racket", SKU = "SPRT-003", Description = "Lightweight tennis racket", UnitPrice = 59.99, CategoryId = _sportsCategoryId },
                new { Id = Guid.NewGuid(), Name = "Yoga Mat", SKU = "SPRT-004", Description = "Non-slip yoga mat", UnitPrice = 24.99, CategoryId = _sportsCategoryId },
                new { Id = Guid.NewGuid(), Name = "Face Cream", SKU = "BEAU-003", Description = "Anti-aging face cream", UnitPrice = 39.99, CategoryId = _beautyCategoryId },
                new { Id = Guid.NewGuid(), Name = "Hair Dryer", SKU = "BEAU-004", Description = "High-speed hair dryer", UnitPrice = 79.99, CategoryId = _beautyCategoryId }
            };

            foreach (var product in products)
            {
                migrationBuilder.Sql($@"
                    INSERT INTO ""Products"" (""Id"", ""Name"", ""SKU"", ""Description"", ""UnitPrice"", ""CategoryId"")
                    VALUES ('{product.Id}', '{product.Name}', '{product.SKU}', '{product.Description}', {product.UnitPrice.ToString("0.00", CultureInfo.InvariantCulture)}, '{product.CategoryId}');
                ");
            }
        }  

        private Guid _inventoryWarehouse1;
        private Guid _inventoryWarehouse2;
        
        private void SeedInventories(MigrationBuilder migrationBuilder)
        {
            _inventoryWarehouse1 = Guid.NewGuid();
            _inventoryWarehouse2 = Guid.NewGuid();
            
            var inventories = new[]
            {
                // Inventory records for the first warehouse (_warehouse1Id)
                new { Id = _inventoryWarehouse1, ProductId = _electronicsProductId, WarehouseId = _warehouse1Id, Quantity = 100 },
                new { Id = Guid.NewGuid(), ProductId = _furnitureProductId, WarehouseId = _warehouse1Id, Quantity = 50 },
                new { Id = Guid.NewGuid(), ProductId = _clothingProductId, WarehouseId = _warehouse1Id, Quantity = 200 },
                new { Id = Guid.NewGuid(), ProductId = _sportsProductId, WarehouseId = _warehouse1Id, Quantity = 150 },
                new { Id = Guid.NewGuid(), ProductId = _beautyProductId, WarehouseId = _warehouse1Id, Quantity = 75 },

                // Inventory records for the second warehouse (_warehouse2Id)
                new { Id = _inventoryWarehouse2, ProductId = _electronicsProductId, WarehouseId = _warehouse2Id, Quantity = 120 },
                new { Id = Guid.NewGuid(), ProductId = _furnitureProductId, WarehouseId = _warehouse2Id, Quantity = 60 },
                new { Id = Guid.NewGuid(), ProductId = _clothingProductId, WarehouseId = _warehouse2Id, Quantity = 180 },
                new { Id = Guid.NewGuid(), ProductId = _sportsProductId, WarehouseId = _warehouse2Id, Quantity = 130 },
                new { Id = Guid.NewGuid(), ProductId = _beautyProductId, WarehouseId = _warehouse2Id, Quantity = 85 }
            };

            foreach (var inventory in inventories)
            {
                migrationBuilder.Sql($@"
                    INSERT INTO ""Inventories"" (""Id"", ""ProductId"", ""WarehouseId"", ""Quantity"")
                    VALUES ('{inventory.Id}', '{inventory.ProductId}', '{inventory.WarehouseId}', {inventory.Quantity});
                ");
            }
        }
        
        private void SeedInventoryLogs(MigrationBuilder migrationBuilder)
        {
            var inventoryLogs = new[]
            {
                new { Id = Guid.NewGuid(), InventoryId = _inventoryWarehouse1, Timestamp = DateTime.UtcNow.AddDays(-10), QuantityChanged = 20, ChangeType = ChangeType.Adjusted },
                new { Id = Guid.NewGuid(), InventoryId = _inventoryWarehouse1, Timestamp = DateTime.UtcNow.AddDays(-8), QuantityChanged = -5, ChangeType = ChangeType.Adjusted },
                new { Id = Guid.NewGuid(), InventoryId = _inventoryWarehouse1, Timestamp = DateTime.UtcNow.AddDays(-6), QuantityChanged = 15, ChangeType = ChangeType.Released },
                new { Id = Guid.NewGuid(), InventoryId = _inventoryWarehouse1, Timestamp = DateTime.UtcNow.AddDays(-4), QuantityChanged = -10, ChangeType = ChangeType.Removed },
                new { Id = Guid.NewGuid(), InventoryId = _inventoryWarehouse2, Timestamp = DateTime.UtcNow.AddDays(-2), QuantityChanged = 30, ChangeType = ChangeType.Added },
                new { Id = Guid.NewGuid(), InventoryId = _inventoryWarehouse2, Timestamp = DateTime.UtcNow.AddDays(-2), QuantityChanged = 30, ChangeType = ChangeType.Added },
            };

            foreach (var log in inventoryLogs)
            {
                migrationBuilder.Sql($@"
                    INSERT INTO ""InventoryLogs"" (""Id"", ""InventoryId"", ""Timestamp"", ""QuantityChanged"", ""ChangeType"")
                    VALUES ('{log.Id}', '{log.InventoryId}', '{log.Timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}', {log.QuantityChanged}, '{log.ChangeType}');
                ");
            }
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
