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

INSERT INTO "Roles" ("Id", "Name") VALUES ('ab457b88-388d-43ba-a6f6-ea952b90fb44','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('cae5aaa6-a1ea-4038-829e-6ca1895141b5','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('93cb369b-6625-49dd-bbcd-f2ff928af465','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('de5a82e5-7b7e-4fc9-88ee-1a6cb74ad9c1', 'Admin User', '1234567890', 'admin@example.com', 'FB30A05E46860E510CFECC2494E5D73923E0397EA54F0B97178F4C30B8180F1B-525AE6B2DD0254C46D2FEEA5C65A7CD4', 'ab457b88-388d-43ba-a6f6-ea952b90fb44')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('a4d70f7a-418e-4d09-ba2d-ad6920d8b788', 'Manager User', '0987654321', 'manager@example.com', '427E3A02AFF3A027EEF52AD98D55DE84E314C2677E44A8F1FBF3EDC226CF073C-0DBD74C3B2F8D7FAA24BF6922A546A3F', 'cae5aaa6-a1ea-4038-829e-6ca1895141b5')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('5913691a-7a96-4217-9864-5912802e09db', 'Worker User', '1112223333', 'worker@example.com', '1AA096A5FB077FDDABEB20CF72F78E5CF1F7E2D7ABB5027C9D74251A27BA8F75-228DD323DBE68CBC5DD3A1E82EFEC8A1', '93cb369b-6625-49dd-bbcd-f2ff928af465')

INSERT INTO "Categories" ("Id", "Name") VALUES ('533a1a7f-1102-4a79-9391-6e5892e097be', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('0ba7b965-5d71-45c5-ae79-d52d98866f13', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('129424ef-1c5c-4e23-8d2d-9fb6e37561af', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('5f4786c8-c754-402b-8442-eebceaf7c849', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('fb9b6942-f56b-4fbd-8bec-2ef04f473998', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('a952501f-701f-4d9d-a565-b6f3c2a0129e', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('d89a5221-4f6b-42b9-a1a6-e2db1c9c6da0', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('4f1f4e2b-3bb7-4b3b-8970-c92e95152256', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '533a1a7f-1102-4a79-9391-6e5892e097be');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('f59ece80-e697-440c-a41f-d5dbdcb9b4c6', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '533a1a7f-1102-4a79-9391-6e5892e097be');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('8a2de30f-cc6c-468d-bb30-b1ec70b8ff35', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, '0ba7b965-5d71-45c5-ae79-d52d98866f13');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('e5f970d3-43a5-4064-8475-cb524e061fd4', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, '0ba7b965-5d71-45c5-ae79-d52d98866f13');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('47a0b5eb-bbb1-4a56-950d-6d8c3fdf92ce', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, '129424ef-1c5c-4e23-8d2d-9fb6e37561af');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('ef5089c3-715c-4c93-8a08-6f948aba7016', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, '129424ef-1c5c-4e23-8d2d-9fb6e37561af');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('09564048-3b26-429f-9985-c8103101cd5b', 'Football', 'SPRT-001', 'Professional football', 29.99, '5f4786c8-c754-402b-8442-eebceaf7c849');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('9d19a801-9fa2-4536-8f54-9b7174e51922', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, '5f4786c8-c754-402b-8442-eebceaf7c849');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9d1f96a-e568-4f7f-a164-181c2e00961b', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, 'fb9b6942-f56b-4fbd-8bec-2ef04f473998');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2986b848-6269-4404-8157-44b8408b1774', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, 'fb9b6942-f56b-4fbd-8bec-2ef04f473998');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('fa5968aa-fadf-45bd-a50d-f9c9e359fd27', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '533a1a7f-1102-4a79-9391-6e5892e097be');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('b74bb73e-08c2-4542-9c28-2d8fb09e967a', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '533a1a7f-1102-4a79-9391-6e5892e097be');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('31519e3f-291f-4735-9c3a-8e4bfa9a54c4', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, '0ba7b965-5d71-45c5-ae79-d52d98866f13');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('4d849b80-e3fa-4158-b447-2ff35254ef82', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, '0ba7b965-5d71-45c5-ae79-d52d98866f13');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('e2e002e4-e09c-44a5-94ce-136d1abc4b89', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, '129424ef-1c5c-4e23-8d2d-9fb6e37561af');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('5404939d-1f28-4bba-8731-e3fb7819ea89', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, '129424ef-1c5c-4e23-8d2d-9fb6e37561af');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('28b16cb1-4210-4c0f-9b3d-636a5155817c', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, '5f4786c8-c754-402b-8442-eebceaf7c849');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('b09fbd57-aa06-4480-9dd7-8afbdc3cf165', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, '5f4786c8-c754-402b-8442-eebceaf7c849');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('4607a0ce-b8ce-4596-976e-12dc2fb33313', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, 'fb9b6942-f56b-4fbd-8bec-2ef04f473998');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('3fbd7400-0f86-4912-8477-08b41c2cd7a4', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, 'fb9b6942-f56b-4fbd-8bec-2ef04f473998');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('cf3f486e-bb1a-4593-90ca-33688517b6c4', '4f1f4e2b-3bb7-4b3b-8970-c92e95152256', 'a952501f-701f-4d9d-a565-b6f3c2a0129e', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('cbf2c4f5-50be-44c3-bddf-2a8220369866', '8a2de30f-cc6c-468d-bb30-b1ec70b8ff35', 'a952501f-701f-4d9d-a565-b6f3c2a0129e', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('c273f6eb-2a22-4679-b95b-1a921f593ac1', '47a0b5eb-bbb1-4a56-950d-6d8c3fdf92ce', 'a952501f-701f-4d9d-a565-b6f3c2a0129e', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('57f4e364-0a24-4304-abcd-9dcffd56dab0', '09564048-3b26-429f-9985-c8103101cd5b', 'a952501f-701f-4d9d-a565-b6f3c2a0129e', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('31f75b3c-2cff-44c5-b051-dd7c574bb4cf', '2986b848-6269-4404-8157-44b8408b1774', 'a952501f-701f-4d9d-a565-b6f3c2a0129e', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('ac499e31-de87-4667-9bfb-f4bfa7ec387f', '4f1f4e2b-3bb7-4b3b-8970-c92e95152256', 'd89a5221-4f6b-42b9-a1a6-e2db1c9c6da0', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('efbbae31-0454-431e-b970-beb7da907658', '8a2de30f-cc6c-468d-bb30-b1ec70b8ff35', 'd89a5221-4f6b-42b9-a1a6-e2db1c9c6da0', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('eb00f35a-9ef2-4ca3-842d-3a598c2ef8f8', '47a0b5eb-bbb1-4a56-950d-6d8c3fdf92ce', 'd89a5221-4f6b-42b9-a1a6-e2db1c9c6da0', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('45705ce4-ac0a-40c4-97c8-1281e12bb613', '09564048-3b26-429f-9985-c8103101cd5b', 'd89a5221-4f6b-42b9-a1a6-e2db1c9c6da0', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('154a7093-a90d-464a-a728-a3b7d7f53f88', '2986b848-6269-4404-8157-44b8408b1774', 'd89a5221-4f6b-42b9-a1a6-e2db1c9c6da0', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('a3df35a8-ccec-4c22-9343-c8aba357bae2', 'cf3f486e-bb1a-4593-90ca-33688517b6c4', '2024-12-24 18:40:37', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('b14e2b3e-3dfb-4716-a076-c40febd02873', 'cf3f486e-bb1a-4593-90ca-33688517b6c4', '2024-12-26 18:40:37', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('16827340-fb75-48df-8f26-605f01e06a95', 'cf3f486e-bb1a-4593-90ca-33688517b6c4', '2024-12-28 18:40:37', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('2f5a2231-13f4-4bce-8cd4-0bbbc7419ad9', 'cf3f486e-bb1a-4593-90ca-33688517b6c4', '2024-12-30 18:40:37', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('3b2b069e-f0a9-4609-9033-1e31772a61dd', 'ac499e31-de87-4667-9bfb-f4bfa7ec387f', '2025-01-01 18:40:37', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('10db545a-186b-4d5d-8b09-bb8673c386e8', 'ac499e31-de87-4667-9bfb-f4bfa7ec387f', '2025-01-01 18:40:37', 30, 'Added');
                

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241222152647_InitialDataSeed', '8.0.2');

COMMIT;

