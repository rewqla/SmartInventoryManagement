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

INSERT INTO "Roles" ("Id", "Name") VALUES ('632e9601-0e3f-b777-987f-87714079ec17','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('632e9601-0e3f-217f-9466-9c415259f9fb','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('632e9601-0e3f-9e79-aac4-9eee8ae715a3','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('632e9601-0f3f-567e-835c-c84ec87ead12', 'Admin User', '1234567890', 'admin@example.com', '118EE716D0A325CEAF8ADCDA4C4A65A9CA04BAC380492F1593BCEEB1FE238716-5CFEE87A721CBB459A8A081D9E23B76A', '632e9601-0e3f-b777-987f-87714079ec17')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('632e9601-2f40-8d70-b69c-c73e0fa602d4', 'Manager User', '0987654321', 'manager@example.com', '895BC1688820B06FAFCC1EB0695611F3CC26738C5F69218E3D43F90A607F5D63-30EBEFC726788424035E566AF807BCB0', '632e9601-0e3f-217f-9466-9c415259f9fb')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('632e9601-4541-a77e-bb6f-720bc2cfd7e6', 'Worker User', '1112223333', 'worker@example.com', '33A311B6B7CFB8E44F18A2DDC8A9057BBE4CB5DD66E19A20CDE21C7D72BBFCF0-EBFBC18716E3B9CEA7761BC8C5045F61', '632e9601-0e3f-9e79-aac4-9eee8ae715a3')

INSERT INTO "Categories" ("Id", "Name") VALUES ('632e9601-5842-a770-9b41-10a24827194f', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('632e9601-5842-b87b-8835-ddfaec4c2e19', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('632e9601-5842-b279-b0cf-368522678aaf', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('632e9601-5842-9c7b-a8e0-5dfabd6a4f53', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('632e9601-5842-eb7c-8599-120e03057f9c', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('632e9601-5842-ce72-be71-053a302f907e', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('632e9601-5842-757e-9a8a-03afd89eb7ef', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-ba72-a2f3-352cd88cb0ef', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '632e9601-5842-a770-9b41-10a24827194f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-d778-a5c8-98d456bb89d0', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '632e9601-5842-a770-9b41-10a24827194f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-0473-8215-44626896bea5', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, '632e9601-5842-b87b-8835-ddfaec4c2e19');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-3273-8508-c805c2f5797c', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, '632e9601-5842-b87b-8835-ddfaec4c2e19');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-da70-a24d-64e8c9a0d01e', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, '632e9601-5842-b279-b0cf-368522678aaf');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-ae74-9c60-946adc442a63', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, '632e9601-5842-b279-b0cf-368522678aaf');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-e578-8e83-a01d771cc269', 'Football', 'SPRT-001', 'Professional football', 29.99, '632e9601-5842-9c7b-a8e0-5dfabd6a4f53');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-dc7b-95e5-059f2c498513', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, '632e9601-5842-9c7b-a8e0-5dfabd6a4f53');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-9e71-94c3-0c0f1db011ed', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, '632e9601-5842-eb7c-8599-120e03057f9c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-4a7e-b1e7-6a91dc79d934', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, '632e9601-5842-eb7c-8599-120e03057f9c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-817f-b3e8-afca9b3f55e1', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '632e9601-5842-a770-9b41-10a24827194f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-a97a-a282-a3cafe4b2137', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '632e9601-5842-a770-9b41-10a24827194f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-7f7e-968d-7ba76f6ffa66', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, '632e9601-5842-b87b-8835-ddfaec4c2e19');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-d27f-9c0c-27aabd7ab53f', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, '632e9601-5842-b87b-8835-ddfaec4c2e19');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-f57e-a093-5ae737275d2d', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, '632e9601-5842-b279-b0cf-368522678aaf');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-ef75-a2d7-fc4b8c4173e4', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, '632e9601-5842-b279-b0cf-368522678aaf');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-6779-bd6c-db6201ac133d', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, '632e9601-5842-9c7b-a8e0-5dfabd6a4f53');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-dc7c-9dc0-27a5a3fa30f7', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, '632e9601-5842-9c7b-a8e0-5dfabd6a4f53');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-0473-9b3f-e7d90cac4e86', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, '632e9601-5842-eb7c-8599-120e03057f9c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('632e9601-5942-b87c-8058-e04c3219381e', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, '632e9601-5842-eb7c-8599-120e03057f9c');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-e47f-b223-b10e430d0cd3', '632e9601-5942-ba72-a2f3-352cd88cb0ef', '632e9601-5842-ce72-be71-053a302f907e', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-ae79-b78a-bd9d1dc9f591', '632e9601-5942-0473-8215-44626896bea5', '632e9601-5842-ce72-be71-053a302f907e', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-207c-9b0d-59cceedefef8', '632e9601-5942-da70-a24d-64e8c9a0d01e', '632e9601-5842-ce72-be71-053a302f907e', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-0c74-b776-09d5c627c81c', '632e9601-5942-e578-8e83-a01d771cc269', '632e9601-5842-ce72-be71-053a302f907e', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-4971-b776-cb64ba4e57ab', '632e9601-5942-4a7e-b1e7-6a91dc79d934', '632e9601-5842-ce72-be71-053a302f907e', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-597e-ad96-96e7b065b696', '632e9601-5942-ba72-a2f3-352cd88cb0ef', '632e9601-5842-757e-9a8a-03afd89eb7ef', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-017f-bc04-200661049ed3', '632e9601-5942-0473-8215-44626896bea5', '632e9601-5842-757e-9a8a-03afd89eb7ef', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-0d7e-8767-be31b52e908c', '632e9601-5942-da70-a24d-64e8c9a0d01e', '632e9601-5842-757e-9a8a-03afd89eb7ef', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-1171-b822-2ce7b3213f2f', '632e9601-5942-e578-8e83-a01d771cc269', '632e9601-5842-757e-9a8a-03afd89eb7ef', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('632e9601-5b42-c476-a309-7a541f0e91f8', '632e9601-5942-4a7e-b1e7-6a91dc79d934', '632e9601-5842-757e-9a8a-03afd89eb7ef', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('632e9601-5b42-267d-bd9f-9ff346521ea1', '632e9601-5b42-e47f-b223-b10e430d0cd3', '2025-04-03 09:02:59', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('632e9601-5c42-7278-8109-5233a5789e15', '632e9601-5b42-e47f-b223-b10e430d0cd3', '2025-04-05 09:02:59', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('632e9601-5c42-127a-be17-24f0741d6c58', '632e9601-5b42-e47f-b223-b10e430d0cd3', '2025-04-07 09:02:59', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('632e9601-5c42-207c-ade8-eeb505b0b847', '632e9601-5b42-e47f-b223-b10e430d0cd3', '2025-04-09 09:02:59', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('632e9601-5c42-1570-a7bf-55b6f4e0cf3e', '632e9601-5b42-597e-ad96-96e7b065b696', '2025-04-11 09:02:59', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('632e9601-5c42-557c-8fd6-c8c11bab31de', '632e9601-5b42-597e-ad96-96e7b065b696', '2025-04-11 09:02:59', 30, 'Added');
                

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

