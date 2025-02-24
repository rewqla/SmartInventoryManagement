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

INSERT INTO "Roles" ("Id", "Name") VALUES ('2c399501-720b-d374-a16b-3751db1ca712','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('2c399501-720b-7f7e-b452-74f0298aa706','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('2c399501-720b-ad70-8627-85dfc2a6e912','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('2c399501-730b-7f79-a049-197eb008fbc6', 'Admin User', '1234567890', 'admin@example.com', 'D22A31CE4DC7F1E1258FED40AD628606C274AB086F511CBBC7F4D824C8007BA5-288CC4A7CE4489A2DFD064D881312F99', '2c399501-720b-d374-a16b-3751db1ca712')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('2c399501-b60c-f176-8c4b-ba1e2ca77b2d', 'Manager User', '0987654321', 'manager@example.com', '33D2ADD70EA74FFC025A8B5DA413B15A95CDDFA282A42E4C19C0096C403D18F8-795E8831AB21D9CD230DFD1B5EB3E6E7', '2c399501-720b-7f7e-b452-74f0298aa706')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('2c399501-e60d-777c-8dbc-4477493c9275', 'Worker User', '1112223333', 'worker@example.com', 'AE337D695DD74CBDEEDA1BE9539C0D3A2C6FE57E502F9F4E1A1D3CF2E93CA365-21115E814E0536770AE561F9C0887975', '2c399501-720b-ad70-8627-85dfc2a6e912')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2c399501-170f-577f-8c62-abb893ae3bb7', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2c399501-170f-4579-bfe3-01aff5824419', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2c399501-170f-fe7d-9fb7-ddf75716e5c1', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2c399501-170f-8b7c-a9f2-fcc0fcf9c1d1', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2c399501-170f-d77c-9d8a-00b8339b3c71', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('2c399501-170f-417a-8f4c-ce7a5ac0b8ea', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('2c399501-170f-cf7b-aae3-117c3ea33a0e', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-407a-bfea-8d97b3fdfda8', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '2c399501-170f-577f-8c62-abb893ae3bb7');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-1278-a302-2ad4b468d788', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '2c399501-170f-577f-8c62-abb893ae3bb7');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-8f73-bbf6-47e94061ac9b', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, '2c399501-170f-4579-bfe3-01aff5824419');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-e477-bfbc-1e0d728a1f7d', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, '2c399501-170f-4579-bfe3-01aff5824419');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-e27c-aefc-f061feb64761', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, '2c399501-170f-fe7d-9fb7-ddf75716e5c1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-8578-9506-0820ff6af557', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, '2c399501-170f-fe7d-9fb7-ddf75716e5c1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-ed75-b3ba-17c10d0bc851', 'Football', 'SPRT-001', 'Professional football', 29.99, '2c399501-170f-8b7c-a9f2-fcc0fcf9c1d1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-a37d-930a-07b367b4d253', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, '2c399501-170f-8b7c-a9f2-fcc0fcf9c1d1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-d975-af9e-390208f0b778', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, '2c399501-170f-d77c-9d8a-00b8339b3c71');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-267b-98b7-355be799a220', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, '2c399501-170f-d77c-9d8a-00b8339b3c71');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-bf7e-abf7-b8b558848c8c', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '2c399501-170f-577f-8c62-abb893ae3bb7');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-d27c-99d3-8c4f7eca086e', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '2c399501-170f-577f-8c62-abb893ae3bb7');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-b571-a60b-fcd2f54e8047', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, '2c399501-170f-4579-bfe3-01aff5824419');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-8f7c-9b55-0f8a63af41f8', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, '2c399501-170f-4579-bfe3-01aff5824419');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-e078-b589-bd7df7ad66de', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, '2c399501-170f-fe7d-9fb7-ddf75716e5c1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-5b78-a2f8-e3a33e03f2c5', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, '2c399501-170f-fe7d-9fb7-ddf75716e5c1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-7070-89eb-9f9c33fff44e', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, '2c399501-170f-8b7c-a9f2-fcc0fcf9c1d1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-8778-bc51-fd9c281fc3b8', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, '2c399501-170f-8b7c-a9f2-fcc0fcf9c1d1');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-ee77-83cd-1191ee61bd87', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, '2c399501-170f-d77c-9d8a-00b8339b3c71');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2c399501-180f-017a-81fe-67d3a1500bc9', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, '2c399501-170f-d77c-9d8a-00b8339b3c71');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-e376-8a43-9d017bf1eb6d', '2c399501-180f-407a-bfea-8d97b3fdfda8', '2c399501-170f-417a-8f4c-ce7a5ac0b8ea', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-1c76-a203-8117ca62963e', '2c399501-180f-8f73-bbf6-47e94061ac9b', '2c399501-170f-417a-8f4c-ce7a5ac0b8ea', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-1273-9bf8-a0a0a0129f1c', '2c399501-180f-e27c-aefc-f061feb64761', '2c399501-170f-417a-8f4c-ce7a5ac0b8ea', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-4678-98a6-661f7d4eb0a1', '2c399501-180f-ed75-b3ba-17c10d0bc851', '2c399501-170f-417a-8f4c-ce7a5ac0b8ea', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-d674-b2f8-7332d8b8db14', '2c399501-180f-267b-98b7-355be799a220', '2c399501-170f-417a-8f4c-ce7a5ac0b8ea', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-1774-8d2a-13b525658739', '2c399501-180f-407a-bfea-8d97b3fdfda8', '2c399501-170f-cf7b-aae3-117c3ea33a0e', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-e572-9672-2e270dfb0bed', '2c399501-180f-8f73-bbf6-47e94061ac9b', '2c399501-170f-cf7b-aae3-117c3ea33a0e', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-bf7c-a34f-9f29fcb52f18', '2c399501-180f-e27c-aefc-f061feb64761', '2c399501-170f-cf7b-aae3-117c3ea33a0e', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-6c74-90b2-ac4e4c07cb33', '2c399501-180f-ed75-b3ba-17c10d0bc851', '2c399501-170f-cf7b-aae3-117c3ea33a0e', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2c399501-1a0f-f575-80e1-b8c18bff106e', '2c399501-180f-267b-98b7-355be799a220', '2c399501-170f-cf7b-aae3-117c3ea33a0e', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2c399501-1a0f-ca72-bedc-ae93a1477a0a', '2c399501-1a0f-e376-8a43-9d017bf1eb6d', '2025-02-14 18:15:43', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2c399501-1a0f-2d7a-af2a-b15565421350', '2c399501-1a0f-e376-8a43-9d017bf1eb6d', '2025-02-16 18:15:43', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2c399501-1a0f-5871-8dd1-6b088aff9de6', '2c399501-1a0f-e376-8a43-9d017bf1eb6d', '2025-02-18 18:15:43', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2c399501-1a0f-3c7a-b05e-13964fa35384', '2c399501-1a0f-e376-8a43-9d017bf1eb6d', '2025-02-20 18:15:43', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2c399501-1a0f-c078-a684-8f954e18c53c', '2c399501-1a0f-1774-8d2a-13b525658739', '2025-02-22 18:15:43', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2c399501-1a0f-7c77-b565-289306ce42a4', '2c399501-1a0f-1774-8d2a-13b525658739', '2025-02-22 18:15:43', 30, 'Added');
                

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

