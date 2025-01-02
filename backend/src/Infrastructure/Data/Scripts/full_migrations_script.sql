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

INSERT INTO "Roles" ("Id", "Name") VALUES ('11349afe-edcc-4ac3-a472-2579b12a9c57','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('96d49a20-d43e-4207-b8bd-d2266484ee75','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('fbd85ef3-21cb-4b2c-adb6-d151467bd20b','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('a00db478-6432-43b9-aed1-18e8857c2cd6', 'Admin User', '1234567890', 'admin@example.com', '69EC61F6033AA377C0B8B73DA72B9FDA88577FC87D21B5AAC0C55ACDA82F09B0-701FEE5146DF49B0B05D48C24C01E7FD', '11349afe-edcc-4ac3-a472-2579b12a9c57')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('9ea512b5-18d1-4ded-a366-31846fd40c36', 'Manager User', '0987654321', 'manager@example.com', 'B1007C1A292922E54FA6D9E0BE807AC23F863A7FE142072399D3283E796B2D84-8539A6F26F666511DD6D90292803AFD3', '96d49a20-d43e-4207-b8bd-d2266484ee75')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('e89356aa-0801-48a2-9e72-8c01eb48a128', 'Worker User', '1112223333', 'worker@example.com', '8059BDAED17F99F63A8997FCDC16FCB696254B9FFE3FFCE216716A3B2DDB2EB6-64E65460039F43074C883CD5F17DFD83', 'fbd85ef3-21cb-4b2c-adb6-d151467bd20b')

INSERT INTO "Categories" ("Id", "Name") VALUES ('1dbe6be9-5615-45b1-a03e-fc3c73c56c7d', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('9a68b058-1f95-4a7f-94d6-c83b9a61b6d6', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('eb886a7c-b0af-4882-8dbb-e59020a88e1f', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('6f28a358-46e6-4157-a132-62301e5dd503', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('5ae73b42-9c26-400b-9552-9828daa79f9d', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('a1d34667-17d8-4405-ae4b-bb0b111ed3a2', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('00e9e55a-c46c-4b45-8b62-52a78936ad64', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('e66499f6-7efc-46bf-8d26-4a565b2966e9', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '1dbe6be9-5615-45b1-a03e-fc3c73c56c7d');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('537dc365-1157-4d13-8692-78b514932671', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '1dbe6be9-5615-45b1-a03e-fc3c73c56c7d');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('65616ead-eabd-4fa7-9cd4-c4e9f546e3c9', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, '9a68b058-1f95-4a7f-94d6-c83b9a61b6d6');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('ddb93f9b-17e5-40bd-8a29-58a14b8df4bd', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, '9a68b058-1f95-4a7f-94d6-c83b9a61b6d6');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('57357d43-8c90-4a52-a334-8921f031b757', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, 'eb886a7c-b0af-4882-8dbb-e59020a88e1f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('1c7d26a1-d192-43f8-aecd-4b644a351d22', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, 'eb886a7c-b0af-4882-8dbb-e59020a88e1f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('bf5a6401-4f71-44a5-9fc2-d817e8d5f628', 'Football', 'SPRT-001', 'Professional football', 29.99, '6f28a358-46e6-4157-a132-62301e5dd503');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('c85fed34-541b-4f9f-8b6e-d7015023e814', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, '6f28a358-46e6-4157-a132-62301e5dd503');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('ebd2b7dd-e292-45fd-a846-de838e8c3979', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, '5ae73b42-9c26-400b-9552-9828daa79f9d');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('54001596-ca63-48e2-bc29-b570962c6bb8', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, '5ae73b42-9c26-400b-9552-9828daa79f9d');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('e0e2c56f-c834-464e-bdbc-1834d5ecff09', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '1dbe6be9-5615-45b1-a03e-fc3c73c56c7d');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('59f0a763-f2a3-4be8-8d98-5f7897065c20', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '1dbe6be9-5615-45b1-a03e-fc3c73c56c7d');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('6183db7f-469b-4d61-8da0-877423b0e3da', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, '9a68b058-1f95-4a7f-94d6-c83b9a61b6d6');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('6c45bf96-6cf9-4c1a-bf87-f8881354a49f', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, '9a68b058-1f95-4a7f-94d6-c83b9a61b6d6');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('564c5e5e-4355-46e9-840c-c41e0ff7bdf9', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, 'eb886a7c-b0af-4882-8dbb-e59020a88e1f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('92fb4294-0c6b-4903-b27d-fc45fc2b1f1d', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, 'eb886a7c-b0af-4882-8dbb-e59020a88e1f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('72f9f28b-0abf-47f0-85a3-1be576231cc6', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, '6f28a358-46e6-4157-a132-62301e5dd503');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('e199ba70-3c82-4a91-a61e-c38b21351133', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, '6f28a358-46e6-4157-a132-62301e5dd503');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('9d333e66-48a1-4c5d-acf3-141e74a65749', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, '5ae73b42-9c26-400b-9552-9828daa79f9d');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('f18db290-781e-46f7-80cf-908c806c79d9', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, '5ae73b42-9c26-400b-9552-9828daa79f9d');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('68fdfaeb-c59f-43c8-89ec-85b9f5b7cd20', 'e66499f6-7efc-46bf-8d26-4a565b2966e9', 'a1d34667-17d8-4405-ae4b-bb0b111ed3a2', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('02fa44db-b6fb-4767-aaef-819e7a13c086', '65616ead-eabd-4fa7-9cd4-c4e9f546e3c9', 'a1d34667-17d8-4405-ae4b-bb0b111ed3a2', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('bc0e8fe7-4d3c-4c7a-a24f-a7c4e4f522aa', '57357d43-8c90-4a52-a334-8921f031b757', 'a1d34667-17d8-4405-ae4b-bb0b111ed3a2', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('b803f160-5b59-48f5-a5a0-f8f27c0d8700', 'bf5a6401-4f71-44a5-9fc2-d817e8d5f628', 'a1d34667-17d8-4405-ae4b-bb0b111ed3a2', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('0dabf6ce-63e8-4f4e-87c7-118935f25460', '54001596-ca63-48e2-bc29-b570962c6bb8', 'a1d34667-17d8-4405-ae4b-bb0b111ed3a2', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('c80b4e51-860b-414f-9052-7e82f13d088c', 'e66499f6-7efc-46bf-8d26-4a565b2966e9', '00e9e55a-c46c-4b45-8b62-52a78936ad64', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('fd662612-419f-47e5-80df-1ee2a510ffbf', '65616ead-eabd-4fa7-9cd4-c4e9f546e3c9', '00e9e55a-c46c-4b45-8b62-52a78936ad64', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('67f674f2-51ee-420d-8686-7476396413f3', '57357d43-8c90-4a52-a334-8921f031b757', '00e9e55a-c46c-4b45-8b62-52a78936ad64', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('f3ed58c7-2c09-4a24-a097-ed2908dc69a8', 'bf5a6401-4f71-44a5-9fc2-d817e8d5f628', '00e9e55a-c46c-4b45-8b62-52a78936ad64', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('5a94e1f9-d6b8-4b7a-9c86-744b05a3efcb', '54001596-ca63-48e2-bc29-b570962c6bb8', '00e9e55a-c46c-4b45-8b62-52a78936ad64', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('da096908-c612-4ecc-bdbd-9f29a353d11a', '68fdfaeb-c59f-43c8-89ec-85b9f5b7cd20', '2024-12-23 18:23:27', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('12fe9166-aa5b-481c-a0b8-1efd6d3af834', '68fdfaeb-c59f-43c8-89ec-85b9f5b7cd20', '2024-12-25 18:23:27', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('a5b77ce7-0b39-46c2-bdcd-e16cfd0848d2', '68fdfaeb-c59f-43c8-89ec-85b9f5b7cd20', '2024-12-27 18:23:27', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('4fff6863-ee14-4416-85eb-2d5fc3a29953', '68fdfaeb-c59f-43c8-89ec-85b9f5b7cd20', '2024-12-29 18:23:27', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('6b4c7bfb-fcf2-4ece-8994-8bd69aeea5a9', 'c80b4e51-860b-414f-9052-7e82f13d088c', '2024-12-31 18:23:27', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('9cb56718-5f1d-4f01-922c-264e05918b40', 'c80b4e51-860b-414f-9052-7e82f13d088c', '2024-12-31 18:23:27', 30, 'Added');
                

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241222152647_InitialDataSeed', '8.0.2');

COMMIT;

