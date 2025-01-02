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

INSERT INTO "Roles" ("Id", "Name") VALUES ('2b43b66a-45a0-463b-b22e-3ccdd66caf78','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('34dc9dca-5b3c-4be0-8cad-4dc15d13ef0c','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('3c8fa1e6-95d7-4bda-b846-7226fc58e767','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('689ac20a-38f6-4c23-95a8-1281fe612d08', 'Admin User', '1234567890', 'admin@example.com', '775AF8AA2560243CE20DCB098BC678FA116F1E9C57614A9A65421DE9F64C4C42-764CAA97C49CC74B41A2D70B9F96A3D8', '2b43b66a-45a0-463b-b22e-3ccdd66caf78')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('7fa66d39-0370-41db-b7c9-7bf45c19d251', 'Manager User', '0987654321', 'manager@example.com', '35B0534243547BB6B9E56F9564FDA351799F3112D6FEC442311E2B873C3507AD-C97B8F23A75C80836F3AD7521A5E982E', '34dc9dca-5b3c-4be0-8cad-4dc15d13ef0c')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('a528a3e3-8511-44f5-88a3-09b0707da181', 'Worker User', '1112223333', 'worker@example.com', '0F96EA782ADDEF5327610F30F03554B08AB7D1217914103B07D164B856703737-FC72EA6B0CF950E0A34F8CAB7E63CDFA', '3c8fa1e6-95d7-4bda-b846-7226fc58e767')

INSERT INTO "Categories" ("Id", "Name") VALUES ('a6eb7fcb-742c-4385-8842-091982fdd227', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('6f8ffe6e-0956-4873-ac55-740ede04ab45', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('e9ccc362-9f83-492a-b17e-87da108c4c5b', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('c8afa565-6922-4c17-9612-51eb1659007c', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('16bd4a54-1daa-4915-b55f-0a277ebfec24', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('d516df52-bef6-4819-95ad-d1e9bff5f8fc', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('34240ea4-5d35-4d30-9814-69314e19a0cb', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('684fb11b-64e5-45a0-9d7f-84d74d0d8176', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, 'a6eb7fcb-742c-4385-8842-091982fdd227');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('eb2b4a62-6591-4cd1-b362-1e1972ec97de', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, 'a6eb7fcb-742c-4385-8842-091982fdd227');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('f27a264f-94f9-43f8-904a-8a60239219fa', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, '6f8ffe6e-0956-4873-ac55-740ede04ab45');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d9aa3e7b-8bc5-485d-a92e-71d5332f62ad', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, '6f8ffe6e-0956-4873-ac55-740ede04ab45');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('f5ab9aef-85bf-470d-a6a6-4a6606554ce0', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, 'e9ccc362-9f83-492a-b17e-87da108c4c5b');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('5f75e0d6-162e-4385-9915-195aff6bc949', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, 'e9ccc362-9f83-492a-b17e-87da108c4c5b');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('f0074b32-c52b-47ff-bfca-76ede61a1fff', 'Football', 'SPRT-001', 'Professional football', 29.99, 'c8afa565-6922-4c17-9612-51eb1659007c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('8687ef8b-f4ee-497b-8610-684cd5ea6bcb', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, 'c8afa565-6922-4c17-9612-51eb1659007c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('6b821863-0544-49c2-bc73-77c88478bbc1', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, '16bd4a54-1daa-4915-b55f-0a277ebfec24');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d28528e1-4df4-48c5-9730-7645ff0c48aa', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, '16bd4a54-1daa-4915-b55f-0a277ebfec24');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('ba329e47-b216-428e-ab2e-2c4c93b2e8d3', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, 'a6eb7fcb-742c-4385-8842-091982fdd227');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('c2bf534d-2ae6-4a08-b921-a67bc21dae65', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, 'a6eb7fcb-742c-4385-8842-091982fdd227');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('b4548c8a-4e8e-4753-919c-2b3503a9ca52', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, '6f8ffe6e-0956-4873-ac55-740ede04ab45');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('cac4172a-234c-46d2-94dd-308f81b181fc', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, '6f8ffe6e-0956-4873-ac55-740ede04ab45');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('b7f242ea-c51a-45f7-a585-2fb80c050d41', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, 'e9ccc362-9f83-492a-b17e-87da108c4c5b');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('021020b5-97cc-4f49-89a2-9d7591c8c056', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, 'e9ccc362-9f83-492a-b17e-87da108c4c5b');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('9e61852f-efbc-4473-b2a4-ed305cb34666', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, 'c8afa565-6922-4c17-9612-51eb1659007c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('dfc5195e-93fd-42d9-b228-51afe19d5e46', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, 'c8afa565-6922-4c17-9612-51eb1659007c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('aa2fa533-7181-423b-af00-8775e5a43748', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, '16bd4a54-1daa-4915-b55f-0a277ebfec24');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('af37b5f3-2f70-4be5-9263-9244d9c0fb57', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, '16bd4a54-1daa-4915-b55f-0a277ebfec24');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('26e16ef1-a51f-40fd-9ff0-d304ef8b2c5e', '684fb11b-64e5-45a0-9d7f-84d74d0d8176', 'd516df52-bef6-4819-95ad-d1e9bff5f8fc', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('5e89c6e4-2cd5-4430-91cc-fc941fefa473', 'f27a264f-94f9-43f8-904a-8a60239219fa', 'd516df52-bef6-4819-95ad-d1e9bff5f8fc', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('5b40e0d5-59e8-4dbf-951c-764b1f08e1aa', 'f5ab9aef-85bf-470d-a6a6-4a6606554ce0', 'd516df52-bef6-4819-95ad-d1e9bff5f8fc', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('0fa25a50-ca22-40ea-acbb-c1cc39366b62', 'f0074b32-c52b-47ff-bfca-76ede61a1fff', 'd516df52-bef6-4819-95ad-d1e9bff5f8fc', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('8a38d2af-0c4a-4a6e-90e4-fbf4e481b1da', 'd28528e1-4df4-48c5-9730-7645ff0c48aa', 'd516df52-bef6-4819-95ad-d1e9bff5f8fc', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('9227e9d6-61c7-439a-ae7c-4e1185cc2dd7', '684fb11b-64e5-45a0-9d7f-84d74d0d8176', '34240ea4-5d35-4d30-9814-69314e19a0cb', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('d85a7faa-0574-4ce0-8e92-6dfdeb5325a9', 'f27a264f-94f9-43f8-904a-8a60239219fa', '34240ea4-5d35-4d30-9814-69314e19a0cb', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('b18c234f-515c-4460-accd-98780b280c7c', 'f5ab9aef-85bf-470d-a6a6-4a6606554ce0', '34240ea4-5d35-4d30-9814-69314e19a0cb', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('aae2b877-ced6-4ef5-a721-e2c358ebe39d', 'f0074b32-c52b-47ff-bfca-76ede61a1fff', '34240ea4-5d35-4d30-9814-69314e19a0cb', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('3f8e9d51-e589-4fda-bd54-a2b47d29c338', 'd28528e1-4df4-48c5-9730-7645ff0c48aa', '34240ea4-5d35-4d30-9814-69314e19a0cb', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('568dc4e9-2077-4f5d-a45a-4e4970770142', '26e16ef1-a51f-40fd-9ff0-d304ef8b2c5e', '2024-12-23 17:45:57', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('351614d0-c95f-459f-814f-e1d7a45f13c7', '26e16ef1-a51f-40fd-9ff0-d304ef8b2c5e', '2024-12-25 17:45:57', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('e619ec79-ec7b-4e8c-8bb9-1277083380bb', '26e16ef1-a51f-40fd-9ff0-d304ef8b2c5e', '2024-12-27 17:45:57', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('db11be86-0a23-4997-acbe-47a27298c92e', '26e16ef1-a51f-40fd-9ff0-d304ef8b2c5e', '2024-12-29 17:45:57', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('ab599842-fb85-4a80-9b1a-13caaea86d09', '9227e9d6-61c7-439a-ae7c-4e1185cc2dd7', '2024-12-31 17:45:57', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('7bc02cb0-ba20-44d6-a3a8-ddaeaf65b7dc', '9227e9d6-61c7-439a-ae7c-4e1185cc2dd7', '2024-12-31 17:45:57', 30, 'Added');
                

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241222152647_InitialDataSeed', '8.0.2');

COMMIT;

