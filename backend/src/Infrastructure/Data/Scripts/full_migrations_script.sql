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

INSERT INTO "Roles" ("Id", "Name") VALUES ('2d529501-2668-147e-a37e-29bfcdeb3f8a','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('2d529501-2668-7f7f-8e59-6e0518b3f111','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('2d529501-2668-1079-8ff7-fdb6b3bcf8b3','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('2d529501-2668-0772-b058-c73f48353589', 'Admin User', '1234567890', 'admin@example.com', 'C03B2C8B5CE3437038ECE5F16B647C1575F667DF69D6E55D70FD45292F5DE3FB-6B43FA8E9231AAAB884AFF73FF1A1B89', '2d529501-2668-147e-a37e-29bfcdeb3f8a')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('2d529501-4169-4a7b-ad03-aaa7cd0e8722', 'Manager User', '0987654321', 'manager@example.com', '0F80ED0A3FCF5224DA5480522A0CB1A879C88BA24EA514364054FC84FBD1370A-323E5765D9C5B589287FB8FAF1970D5D', '2d529501-2668-7f7f-8e59-6e0518b3f111')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('2d529501-576a-1f75-b5ef-8d79de3957f8', 'Worker User', '1112223333', 'worker@example.com', 'A523082F9F4E977C501E6D47D2454D0ADB3059ABEEEC412875EF7FAA3F8182F0-3684808D3C0F4A71CE2A53352A00E8D8', '2d529501-2668-1079-8ff7-fdb6b3bcf8b3')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2d529501-6f6b-0572-8906-060aea3e74a5', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2d529501-6f6b-b97a-a780-e32065135985', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2d529501-6f6b-767d-93a1-c2f061033aba', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2d529501-6f6b-7573-abab-fc5dfca6acf8', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2d529501-6f6b-4f73-8eef-ec233dca22f0', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('2d529501-6f6b-b67c-bee7-d31b4ed790db', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('2d529501-6f6b-a578-98fd-cbffad3c7ee2', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-f673-84be-c45c501e0daa', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '2d529501-6f6b-0572-8906-060aea3e74a5');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-ed75-bc0c-18f52d728924', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '2d529501-6f6b-0572-8906-060aea3e74a5');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-1173-b3a6-e52e04d575f9', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, '2d529501-6f6b-b97a-a780-e32065135985');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-dd77-8c01-8ef8dd9e502e', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, '2d529501-6f6b-b97a-a780-e32065135985');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-447c-9c99-e2e3ecb9df81', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, '2d529501-6f6b-767d-93a1-c2f061033aba');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-a97a-a22d-3166e4c2368b', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, '2d529501-6f6b-767d-93a1-c2f061033aba');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-bb74-bdeb-a3361c1f8159', 'Football', 'SPRT-001', 'Professional football', 29.99, '2d529501-6f6b-7573-abab-fc5dfca6acf8');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-3570-9512-b2fc74a4573d', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, '2d529501-6f6b-7573-abab-fc5dfca6acf8');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-4977-9374-76734cea405f', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, '2d529501-6f6b-4f73-8eef-ec233dca22f0');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-647c-9ebc-9aa02a5afbf1', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, '2d529501-6f6b-4f73-8eef-ec233dca22f0');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-4e72-b949-c8a9b93167df', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '2d529501-6f6b-0572-8906-060aea3e74a5');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-8871-b4d8-bc2d7a58b8b9', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '2d529501-6f6b-0572-8906-060aea3e74a5');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-e67d-82cd-3723cc1c9f64', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, '2d529501-6f6b-b97a-a780-e32065135985');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-f572-b2a8-991e31250ccb', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, '2d529501-6f6b-b97a-a780-e32065135985');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-187c-bb60-9e3aebf9b49a', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, '2d529501-6f6b-767d-93a1-c2f061033aba');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-3373-87e7-6c670b8fc987', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, '2d529501-6f6b-767d-93a1-c2f061033aba');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-0c7c-b2be-a9b1c0651e65', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, '2d529501-6f6b-7573-abab-fc5dfca6acf8');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-a87c-84dc-6904758d638a', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, '2d529501-6f6b-7573-abab-fc5dfca6acf8');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-ba79-9a0d-70058060fdaf', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, '2d529501-6f6b-4f73-8eef-ec233dca22f0');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2d529501-706b-5273-90a4-3eb424f98ec9', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, '2d529501-6f6b-4f73-8eef-ec233dca22f0');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-de78-b80e-520368755da6', '2d529501-706b-f673-84be-c45c501e0daa', '2d529501-6f6b-b67c-bee7-d31b4ed790db', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-3372-8017-532c109282e9', '2d529501-706b-1173-b3a6-e52e04d575f9', '2d529501-6f6b-b67c-bee7-d31b4ed790db', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-8f74-937a-489afb859557', '2d529501-706b-447c-9c99-e2e3ecb9df81', '2d529501-6f6b-b67c-bee7-d31b4ed790db', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-777d-a015-dc6b70cc5048', '2d529501-706b-bb74-bdeb-a3361c1f8159', '2d529501-6f6b-b67c-bee7-d31b4ed790db', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-587a-b6f8-dd609fe130ed', '2d529501-706b-647c-9ebc-9aa02a5afbf1', '2d529501-6f6b-b67c-bee7-d31b4ed790db', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-f673-9fdc-b6819a69ebad', '2d529501-706b-f673-84be-c45c501e0daa', '2d529501-6f6b-a578-98fd-cbffad3c7ee2', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-ed7b-ad99-c388150c937f', '2d529501-706b-1173-b3a6-e52e04d575f9', '2d529501-6f6b-a578-98fd-cbffad3c7ee2', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-b475-bfc6-93772e23c898', '2d529501-706b-447c-9c99-e2e3ecb9df81', '2d529501-6f6b-a578-98fd-cbffad3c7ee2', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-6c7a-a2f8-baa1f7c9d7ee', '2d529501-706b-bb74-bdeb-a3361c1f8159', '2d529501-6f6b-a578-98fd-cbffad3c7ee2', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2d529501-716b-9f77-9312-b120fc8f5fd8', '2d529501-706b-647c-9ebc-9aa02a5afbf1', '2d529501-6f6b-a578-98fd-cbffad3c7ee2', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2d529501-726b-ae72-ac4c-7f16aac9bf55', '2d529501-716b-de78-b80e-520368755da6', '2025-02-19 14:47:43', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2d529501-726b-c877-9c3d-4717b672d546', '2d529501-716b-de78-b80e-520368755da6', '2025-02-21 14:47:43', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2d529501-726b-e87f-ba1a-80aa1730cc68', '2d529501-716b-de78-b80e-520368755da6', '2025-02-23 14:47:43', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2d529501-726b-a67f-8068-05538fb9c957', '2d529501-716b-de78-b80e-520368755da6', '2025-02-25 14:47:43', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2d529501-726b-1c75-9cc8-b88799582d78', '2d529501-716b-f673-9fdc-b6819a69ebad', '2025-02-27 14:47:43', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2d529501-726b-b272-8d42-1a089a370a90', '2d529501-716b-f673-9fdc-b6819a69ebad', '2025-02-27 14:47:43', 30, 'Added');
                

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

