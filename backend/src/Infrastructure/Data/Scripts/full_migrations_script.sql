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

INSERT INTO "Roles" ("Id", "Name") VALUES ('104a2384-7e6e-408e-b11b-a64e300530d3','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('06cf9e40-f9db-4ab9-87ac-b9a13d7f97a3','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('930341d0-77a4-467c-a437-55e73dee2632','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('5ed1af34-b5ad-4f6f-aaa8-7a3f70eb3905', 'Admin User', '1234567890', 'admin@example.com', '4505839E9B9EF0BC344C8FFCEDE434AF9F18A6790E6B7D3E4A33BA12E1FF7840-030714FD005565AC5995713239CEFF5A', '104a2384-7e6e-408e-b11b-a64e300530d3')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('57e59663-f4c2-4867-b805-fd12653297b1', 'Manager User', '0987654321', 'manager@example.com', '09F2ACB2F01A80BD1237F5362F25E68A142FC12BDB4D0C1C0FCAF6C1A094022D-131CF48E7D577712DD767AE26B96490B', '06cf9e40-f9db-4ab9-87ac-b9a13d7f97a3')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('9c1e44c0-c042-4749-9025-30a2cf4729b7', 'Worker User', '1112223333', 'worker@example.com', '4EE57C9E368CA70D3EC44C1B190545A42330B51427C21155D24DF08380BAE990-B105510993BC7F0E6F4A9EEA9D1E4B98', '930341d0-77a4-467c-a437-55e73dee2632')

INSERT INTO "Categories" ("Id", "Name") VALUES ('7b60ace0-9876-4a10-9347-8f786061c18c', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('c4af9782-cd4a-489a-a699-6b700bf2dab4', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('3f70affe-5a68-4e87-9b5d-12f18ddef631', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('e7179b17-75d1-4caa-b7de-1907ee140957', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('0b56197f-4c30-47f7-8508-7c52a6364c06', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('37af8801-96b3-49d4-8b06-126026aa40ba', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('aba3c1b9-85ff-4bbe-a66f-1ce9a6f66a78', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('bfb31566-4e8b-4c8a-adf0-f34671a27635', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '7b60ace0-9876-4a10-9347-8f786061c18c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('9131150e-d98f-4ef9-8a1d-a55ca440e16a', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '7b60ace0-9876-4a10-9347-8f786061c18c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('8ba2fc5e-79a2-4bec-b7e5-0d722ed37fce', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, 'c4af9782-cd4a-489a-a699-6b700bf2dab4');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('beb872ae-9aab-4529-a10c-3d298e2d6f11', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, 'c4af9782-cd4a-489a-a699-6b700bf2dab4');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('48cfac42-393d-4bd9-bebc-14135503c823', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, '3f70affe-5a68-4e87-9b5d-12f18ddef631');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('a41b8247-738f-4648-bc72-98f38e8dcd05', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, '3f70affe-5a68-4e87-9b5d-12f18ddef631');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('71acb451-924f-4d33-bf34-fcd357d90846', 'Football', 'SPRT-001', 'Professional football', 29.99, 'e7179b17-75d1-4caa-b7de-1907ee140957');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('179ad281-b2fc-4cfa-b37e-5a135e9d6d7e', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, 'e7179b17-75d1-4caa-b7de-1907ee140957');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('67433718-d293-4045-877a-0daa9b23b884', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, '0b56197f-4c30-47f7-8508-7c52a6364c06');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d117ce90-940f-497e-bc5b-068404b814af', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, '0b56197f-4c30-47f7-8508-7c52a6364c06');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('03c6eb50-ae23-4f45-b208-3ac3e975968b', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '7b60ace0-9876-4a10-9347-8f786061c18c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('a785d24c-a1f3-4893-98ee-8448e092c3ab', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '7b60ace0-9876-4a10-9347-8f786061c18c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('c45cb9b1-8f12-4694-b26b-256797411b3d', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, 'c4af9782-cd4a-489a-a699-6b700bf2dab4');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('80109140-25d5-43e3-a1de-c8a118c84444', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, 'c4af9782-cd4a-489a-a699-6b700bf2dab4');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d3426ffc-4ada-41bc-bb0b-bd7047b6df41', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, '3f70affe-5a68-4e87-9b5d-12f18ddef631');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('0f5a5baf-a6d2-4cbf-b469-5a014210f27b', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, '3f70affe-5a68-4e87-9b5d-12f18ddef631');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('684c8995-746c-4060-a693-3a4fa6fdff25', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, 'e7179b17-75d1-4caa-b7de-1907ee140957');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('6919bd21-852c-4712-ad15-1ff53c3817c2', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, 'e7179b17-75d1-4caa-b7de-1907ee140957');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('14408232-9504-4493-8d5b-aa051aa9fc6b', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, '0b56197f-4c30-47f7-8508-7c52a6364c06');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('844c49b1-e3cf-4110-ac94-1ac583f08007', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, '0b56197f-4c30-47f7-8508-7c52a6364c06');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('841c64a3-0599-4cab-b3b7-97906335fc28', 'bfb31566-4e8b-4c8a-adf0-f34671a27635', '37af8801-96b3-49d4-8b06-126026aa40ba', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('ae921c4b-7c62-4970-9a05-ff8a7f0bcf26', '8ba2fc5e-79a2-4bec-b7e5-0d722ed37fce', '37af8801-96b3-49d4-8b06-126026aa40ba', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('4363e63e-c294-497a-ab73-f905dd6c3f93', '48cfac42-393d-4bd9-bebc-14135503c823', '37af8801-96b3-49d4-8b06-126026aa40ba', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('907843e4-c251-4ef7-872c-3f5125bd9a47', '71acb451-924f-4d33-bf34-fcd357d90846', '37af8801-96b3-49d4-8b06-126026aa40ba', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('ce4da199-92e6-436b-ab5e-6bcc0552acf2', 'd117ce90-940f-497e-bc5b-068404b814af', '37af8801-96b3-49d4-8b06-126026aa40ba', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('9b17758f-79a5-4044-9412-465e846c3380', 'bfb31566-4e8b-4c8a-adf0-f34671a27635', 'aba3c1b9-85ff-4bbe-a66f-1ce9a6f66a78', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('825ff143-ccf0-4d6b-955f-cf8d1148d6df', '8ba2fc5e-79a2-4bec-b7e5-0d722ed37fce', 'aba3c1b9-85ff-4bbe-a66f-1ce9a6f66a78', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('88ae6e68-b8e2-418e-a03d-db1e2ab78b3f', '48cfac42-393d-4bd9-bebc-14135503c823', 'aba3c1b9-85ff-4bbe-a66f-1ce9a6f66a78', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('f9a3efa5-d578-48ad-a7a1-80212f180dc3', '71acb451-924f-4d33-bf34-fcd357d90846', 'aba3c1b9-85ff-4bbe-a66f-1ce9a6f66a78', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('1d9e5d5e-40df-40ff-b84d-b67188e0e672', 'd117ce90-940f-497e-bc5b-068404b814af', 'aba3c1b9-85ff-4bbe-a66f-1ce9a6f66a78', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('5af32770-9402-49bb-bd0e-ba1587ddbb5c', '841c64a3-0599-4cab-b3b7-97906335fc28', '2024-12-25 10:55:51', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('6ff858a8-dc4c-47ef-94c4-2d047726930f', '841c64a3-0599-4cab-b3b7-97906335fc28', '2024-12-27 10:55:51', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('8a7f431e-24f2-4376-978b-6d7f0b233868', '841c64a3-0599-4cab-b3b7-97906335fc28', '2024-12-29 10:55:51', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('792a13aa-303f-4bce-b906-fc1f7e1d3b17', '841c64a3-0599-4cab-b3b7-97906335fc28', '2024-12-31 10:55:51', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('9c8fcc84-1987-4d7c-9df7-5f7cfe8406e5', '9b17758f-79a5-4044-9412-465e846c3380', '2025-01-02 10:55:51', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('3357cd40-a75e-4202-83c6-67c14ae2a743', '9b17758f-79a5-4044-9412-465e846c3380', '2025-01-02 10:55:51', 30, 'Added');
                

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

