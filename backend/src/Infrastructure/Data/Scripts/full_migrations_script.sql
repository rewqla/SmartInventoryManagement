CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE public."Categories" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    CONSTRAINT "PK_Categories" PRIMARY KEY ("Id")
);

CREATE TABLE public."Roles" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
);

CREATE TABLE public."Warehouses" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Location" text NOT NULL,
    CONSTRAINT "PK_Warehouses" PRIMARY KEY ("Id")
);

CREATE TABLE public."Products" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "SKU" text NOT NULL,
    "Description" text,
    "UnitPrice" double precision NOT NULL,
    "CategoryId" uuid NOT NULL,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Products_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES public."Categories" ("Id") ON DELETE CASCADE
);

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Phone" text NOT NULL,
    "Email" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "RoleId" uuid NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Users_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES public."Roles" ("Id") ON DELETE CASCADE
);

CREATE TABLE public."Inventories" (
    "Id" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "WarehouseId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    CONSTRAINT "PK_Inventories" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Inventories_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES public."Products" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Inventories_Warehouses_WarehouseId" FOREIGN KEY ("WarehouseId") REFERENCES public."Warehouses" ("Id") ON DELETE CASCADE
);

CREATE TABLE public."InventoryLogs" (
    "Id" uuid NOT NULL,
    "InventoryId" uuid NOT NULL,
    "Timestamp" timestamp with time zone NOT NULL,
    "QuantityChanged" integer NOT NULL,
    "ChangeType" text NOT NULL,
    CONSTRAINT "PK_InventoryLogs" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_InventoryLogs_Inventories_InventoryId" FOREIGN KEY ("InventoryId") REFERENCES public."Inventories" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Inventories_ProductId" ON public."Inventories" ("ProductId");

CREATE INDEX "IX_Inventories_WarehouseId" ON public."Inventories" ("WarehouseId");

CREATE INDEX "IX_InventoryLogs_InventoryId" ON public."InventoryLogs" ("InventoryId");

CREATE INDEX "IX_Products_CategoryId" ON public."Products" ("CategoryId");

CREATE UNIQUE INDEX "IX_Products_SKU" ON public."Products" ("SKU");

CREATE UNIQUE INDEX "IX_Users_Email" ON public."Users" ("Email");

CREATE UNIQUE INDEX "IX_Users_Phone" ON public."Users" ("Phone");

CREATE INDEX "IX_Users_RoleId" ON public."Users" ("RoleId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241222152422_Initial', '8.0.2');

COMMIT;

START TRANSACTION;

INSERT INTO "Roles" ("Id", "Name") VALUES ('d9029a01-1945-8d71-8b21-a3c82fe86dfd','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('d9029a01-1945-c17b-9635-d49882c96065','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('d9029a01-1945-9a78-a79c-f849d335d4f8','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('d9029a01-1a45-9373-9369-51ca632cc0e8', 'Admin User', '1234567890', 'admin@example.com', '0CB08482865F354B0AFAE7451A4FF2C5ABEC6C45A1A774735422970B7C4A6FCF-3A3879572C10ED96C73494576D57D77E', 'd9029a01-1945-8d71-8b21-a3c82fe86dfd')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('d9029a01-5f46-2e7d-bcb5-86226948133d', 'Manager User', '0987654321', 'manager@example.com', '64A26EB9107F6FD70BAE2C3141CB7FAD9336E0F6C359560CD74C3749B9B0A76E-23F7317C7D3E333970E522B70524FE73', 'd9029a01-1945-c17b-9635-d49882c96065')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('d9029a01-e447-4d7f-bae8-59110dc26a25', 'Worker User', '1112223333', 'worker@example.com', '7DE69BB8DA368F2E747A929646AFB4656BCDE428D706DA5595BD1826921A1E47-7401B3530199D2B5B6879640AA0649FA', 'd9029a01-1945-9a78-a79c-f849d335d4f8')

INSERT INTO "Categories" ("Id", "Name") VALUES ('d9029a01-b349-4379-a11b-3da4b08c0ecb', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('d9029a01-b349-ce79-8be7-d190ecee1250', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('d9029a01-b349-4b7d-b067-415504c22d52', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('d9029a01-b349-8674-82ae-0f0b5974ba91', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('d9029a01-b349-347a-821f-4852a34207f0', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('d9029a01-b349-3c75-b02c-95e08808bd10', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('d9029a01-b349-c67b-89c6-53f72af74cf4', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-247d-8776-60f1630ca4d8', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, 'd9029a01-b349-4379-a11b-3da4b08c0ecb');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-d074-805d-51f9da43540c', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, 'd9029a01-b349-4379-a11b-3da4b08c0ecb');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-9577-b011-2db621debefc', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, 'd9029a01-b349-ce79-8be7-d190ecee1250');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-8c75-8422-7cdf20b76f55', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, 'd9029a01-b349-ce79-8be7-d190ecee1250');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-0e7c-b91f-607eda782c7f', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, 'd9029a01-b349-4b7d-b067-415504c22d52');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-c570-925e-cc265f721252', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, 'd9029a01-b349-4b7d-b067-415504c22d52');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-3272-9445-f8e55064c878', 'Football', 'SPRT-001', 'Professional football', 29.99, 'd9029a01-b349-8674-82ae-0f0b5974ba91');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-9070-b2b7-4a2519d24d22', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, 'd9029a01-b349-8674-82ae-0f0b5974ba91');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-f676-b269-5f4c1548440b', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, 'd9029a01-b349-347a-821f-4852a34207f0');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-ac73-80b7-567343ed1034', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, 'd9029a01-b349-347a-821f-4852a34207f0');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-0c7c-ad3f-fdd7a698ce48', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, 'd9029a01-b349-4379-a11b-3da4b08c0ecb');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-3272-8050-e6388fed2948', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, 'd9029a01-b349-4379-a11b-3da4b08c0ecb');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-9972-8a4d-4cd9f1adf453', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, 'd9029a01-b349-ce79-8be7-d190ecee1250');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-a778-a447-f9db6ad700b6', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, 'd9029a01-b349-ce79-8be7-d190ecee1250');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-0170-aaec-2037917875f7', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, 'd9029a01-b349-4b7d-b067-415504c22d52');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-f579-ab77-0bb7140266fc', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, 'd9029a01-b349-4b7d-b067-415504c22d52');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-0f74-9081-567307e79dbe', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, 'd9029a01-b349-8674-82ae-0f0b5974ba91');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-c27d-bfd6-f1cc76a033c1', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, 'd9029a01-b349-8674-82ae-0f0b5974ba91');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-2e79-89fd-a24a70db106f', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, 'd9029a01-b349-347a-821f-4852a34207f0');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9029a01-b449-4d73-a54b-ddf433909e82', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, 'd9029a01-b349-347a-821f-4852a34207f0');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-d570-9250-422ec88fecce', 'd9029a01-b449-247d-8776-60f1630ca4d8', 'd9029a01-b349-3c75-b02c-95e08808bd10', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-967d-a67e-ad8ae6fc2170', 'd9029a01-b449-9577-b011-2db621debefc', 'd9029a01-b349-3c75-b02c-95e08808bd10', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-cd7b-a85f-a7f2a5df047e', 'd9029a01-b449-0e7c-b91f-607eda782c7f', 'd9029a01-b349-3c75-b02c-95e08808bd10', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-6577-8c9b-4cc3a6c3853a', 'd9029a01-b449-3272-9445-f8e55064c878', 'd9029a01-b349-3c75-b02c-95e08808bd10', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-3e72-9da6-048fe1127dcf', 'd9029a01-b449-ac73-80b7-567343ed1034', 'd9029a01-b349-3c75-b02c-95e08808bd10', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-ed7e-8e94-521a5444c0a0', 'd9029a01-b449-247d-8776-60f1630ca4d8', 'd9029a01-b349-c67b-89c6-53f72af74cf4', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-4270-b510-a8a525fbe6e9', 'd9029a01-b449-9577-b011-2db621debefc', 'd9029a01-b349-c67b-89c6-53f72af74cf4', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-f072-af91-0cb088ca5f35', 'd9029a01-b449-0e7c-b91f-607eda782c7f', 'd9029a01-b349-c67b-89c6-53f72af74cf4', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-f375-bddb-d8eb01ee9ff6', 'd9029a01-b449-3272-9445-f8e55064c878', 'd9029a01-b349-c67b-89c6-53f72af74cf4', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d9029a01-b649-fb75-905a-40bcb99ae4ed', 'd9029a01-b449-ac73-80b7-567343ed1034', 'd9029a01-b349-c67b-89c6-53f72af74cf4', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('d9029a01-b749-077c-95a5-bee4b0f724a8', 'd9029a01-b649-d570-9250-422ec88fecce', '2025-10-10 18:19:45', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('d9029a01-b749-f57f-a692-a569715fd6a7', 'd9029a01-b649-d570-9250-422ec88fecce', '2025-10-12 18:19:45', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('d9029a01-b749-6376-b269-ed456167c6d9', 'd9029a01-b649-d570-9250-422ec88fecce', '2025-10-14 18:19:45', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('d9029a01-b749-cd71-81ff-b82917cb005b', 'd9029a01-b649-d570-9250-422ec88fecce', '2025-10-16 18:19:45', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('d9029a01-b749-f370-b002-8b6f6ffa76b1', 'd9029a01-b649-ed7e-8e94-521a5444c0a0', '2025-10-18 18:19:45', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('d9029a01-b749-047a-8b32-041997c48754', 'd9029a01-b649-ed7e-8e94-521a5444c0a0', '2025-10-18 18:19:45', 30, 'Added');
                

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241222152647_InitialDataSeed', '8.0.2');

COMMIT;

START TRANSACTION;

CREATE TABLE public."Tests" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Text" text NOT NULL,
    "DateTime" timestamp with time zone NOT NULL,
    "Age" integer NOT NULL,
    "Persents" real NOT NULL,
    CONSTRAINT "PK_Tests" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250104105416_TestMigration', '8.0.2');

COMMIT;

START TRANSACTION;

CREATE TABLE public."RefreshTokens" (
    "Id" uuid NOT NULL,
    "Token" character varying(256) NOT NULL,
    "ExpiresOnUtc" timestamp with time zone NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RefreshTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_RefreshTokens_Token" ON public."RefreshTokens" ("Token");

CREATE INDEX "IX_RefreshTokens_UserId" ON public."RefreshTokens" ("UserId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250224181211_RefreshTokens', '8.0.2');

COMMIT;

START TRANSACTION;

ALTER TABLE public."Users" RENAME COLUMN "Phone" TO "PhoneNumber";

ALTER TABLE public."Users" RENAME COLUMN "Name" TO "FullName";

ALTER INDEX public."IX_Users_Phone" RENAME TO "IX_Users_PhoneNumber";

ALTER TABLE public."Users" ADD "CreatedAt" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

ALTER TABLE public."Users" ADD "FailedLoginAttempts" integer NOT NULL DEFAULT 0;

ALTER TABLE public."Users" ADD "LockoutEnd" timestamp with time zone;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250413090137_UpdatedUserEntity', '8.0.2');

COMMIT;

START TRANSACTION;

ALTER TABLE public."InventoryLogs" ADD "ChangedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

CREATE INDEX "IX_InventoryLogs_ChangedById" ON public."InventoryLogs" ("ChangedById");

ALTER TABLE public."InventoryLogs" ADD CONSTRAINT "FK_InventoryLogs_Users_ChangedById" FOREIGN KEY ("ChangedById") REFERENCES public."Users" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251020181725_InventoryLogNavigationProperty', '8.0.2');

COMMIT;

