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

INSERT INTO "Roles" ("Id", "Name") VALUES ('23cbbf1c-d133-4e4e-a4ce-7fff579c2501','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('9fa4abcb-c262-4cd2-9cf5-847f305e1603','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('fda2b285-c071-4598-b4ee-592534815307','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('13b24066-c66e-488f-8c8e-eff3aa737b50', 'Admin User', '1234567890', 'admin@example.com', '7DD0E7095D0A2DBA39BA36D5654C3FA3C23C21B12F4FAE7497272E65DD81B8EE-25FA83267C0894B7546532E806346621', '23cbbf1c-d133-4e4e-a4ce-7fff579c2501')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('e4c514e7-8e66-473d-a36c-25ae06133484', 'Manager User', '0987654321', 'manager@example.com', '7990A5A9C882695141A4CF3655A2F61BF69A7A11C8394479B267A95865F02E0E-D3E31D1136764195352C6057F8837446', '9fa4abcb-c262-4cd2-9cf5-847f305e1603')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('57bddb6b-6951-4377-b954-07cbefd3722e', 'Worker User', '1112223333', 'worker@example.com', '3137D63778FE1A759D6EC8C8D3C049A8D06D3E58B099E794CA99E9A81CCC9D9E-7783D0E210E44BDDAA1F3347715D66D5', 'fda2b285-c071-4598-b4ee-592534815307')

INSERT INTO "Categories" ("Id", "Name") VALUES ('2973d9e4-56fa-4f1b-b66e-a23300a44818', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('26ba508d-08a3-44e1-a518-a8241313dc0f', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('e760a986-571a-4e16-9f82-bd7e17b691ff', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('e4082ee0-5bfa-47c2-a1ac-e2d3628c173c', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('b856cfb7-933a-4c93-a11c-4647c35187dd', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('d5d021e1-498b-40bb-8802-0f8c589ac9c0', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('3bb0d482-28fa-41b1-b868-8cd5d356be8c', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('a065d699-3167-49b7-8fc5-a3d0051f1916', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '2973d9e4-56fa-4f1b-b66e-a23300a44818');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('2ac5550f-a7fa-47ae-a66c-a7f4d6857abf', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '2973d9e4-56fa-4f1b-b66e-a23300a44818');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('7f300c91-4c1c-47eb-bad3-b5ce40585c81', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, '26ba508d-08a3-44e1-a518-a8241313dc0f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('b6d13e95-486a-43de-b1b9-a9f31e1700be', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, '26ba508d-08a3-44e1-a518-a8241313dc0f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d454d3a5-c59a-43eb-9a5d-9648480fb0dc', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, 'e760a986-571a-4e16-9f82-bd7e17b691ff');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('fddca9c5-629c-44b5-8bc0-9a59d7289b68', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, 'e760a986-571a-4e16-9f82-bd7e17b691ff');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('4be6f173-528f-494e-8acd-aa3953d331d8', 'Football', 'SPRT-001', 'Professional football', 29.99, 'e4082ee0-5bfa-47c2-a1ac-e2d3628c173c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('50fe9472-2c87-4cf1-a5dd-2de267ebd98a', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, 'e4082ee0-5bfa-47c2-a1ac-e2d3628c173c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('05656d53-a73f-4c4a-9998-564976d3beae', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, 'b856cfb7-933a-4c93-a11c-4647c35187dd');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('b21b0335-c891-482d-9581-932abeda61a7', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, 'b856cfb7-933a-4c93-a11c-4647c35187dd');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('aaa69e8b-f4d2-4b24-9550-43e8898cbdf0', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '2973d9e4-56fa-4f1b-b66e-a23300a44818');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('9831c431-a655-42b6-8eb3-6d537c812f31', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '2973d9e4-56fa-4f1b-b66e-a23300a44818');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('5c6e0b7c-b3fb-409d-9b01-a96a60ad72a2', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, '26ba508d-08a3-44e1-a518-a8241313dc0f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('1c346ab7-f3d9-4d1b-918d-0fba70e25134', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, '26ba508d-08a3-44e1-a518-a8241313dc0f');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('035e69f2-ed6c-4515-acbc-543068924571', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, 'e760a986-571a-4e16-9f82-bd7e17b691ff');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('110a9199-92c5-4c4f-aa1f-4264fa3d0b0b', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, 'e760a986-571a-4e16-9f82-bd7e17b691ff');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('fa2ab0d3-1105-451e-bc9d-0a680fc3014c', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, 'e4082ee0-5bfa-47c2-a1ac-e2d3628c173c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('3457d2e8-f039-43da-98c0-d8ee2fdd1c67', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, 'e4082ee0-5bfa-47c2-a1ac-e2d3628c173c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('4e69da6c-e77a-437f-b42e-94a4b1e967bc', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, 'b856cfb7-933a-4c93-a11c-4647c35187dd');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('cc9880ed-320a-4eee-9309-b7ad2e623a76', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, 'b856cfb7-933a-4c93-a11c-4647c35187dd');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('8dbd4190-e4b7-4eab-a653-d1e78316b57f', 'a065d699-3167-49b7-8fc5-a3d0051f1916', 'd5d021e1-498b-40bb-8802-0f8c589ac9c0', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('6ad7d484-ee06-4c29-ba7d-c5a0e3526063', '7f300c91-4c1c-47eb-bad3-b5ce40585c81', 'd5d021e1-498b-40bb-8802-0f8c589ac9c0', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('0f3f88c7-910a-4928-b3bf-605589d5732c', 'd454d3a5-c59a-43eb-9a5d-9648480fb0dc', 'd5d021e1-498b-40bb-8802-0f8c589ac9c0', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2f06f4cb-f3f5-4561-8eca-3f32704e5ea9', '4be6f173-528f-494e-8acd-aa3953d331d8', 'd5d021e1-498b-40bb-8802-0f8c589ac9c0', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('882a72dd-78e1-43fe-960d-2ce2fa56ab3e', 'b21b0335-c891-482d-9581-932abeda61a7', 'd5d021e1-498b-40bb-8802-0f8c589ac9c0', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('6e8f98b3-e4c7-498e-a92b-b2927aeb8dd2', 'a065d699-3167-49b7-8fc5-a3d0051f1916', '3bb0d482-28fa-41b1-b868-8cd5d356be8c', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('924140d8-9374-4b69-95bd-68f60e5a860d', '7f300c91-4c1c-47eb-bad3-b5ce40585c81', '3bb0d482-28fa-41b1-b868-8cd5d356be8c', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('2391d07e-4dfb-4c43-b02b-97e712191c90', 'd454d3a5-c59a-43eb-9a5d-9648480fb0dc', '3bb0d482-28fa-41b1-b868-8cd5d356be8c', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('0f7c649f-cb2c-4961-921d-d75fd65108ef', '4be6f173-528f-494e-8acd-aa3953d331d8', '3bb0d482-28fa-41b1-b868-8cd5d356be8c', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('a4aca32f-e767-4036-b51f-aca018cb82c2', 'b21b0335-c891-482d-9581-932abeda61a7', '3bb0d482-28fa-41b1-b868-8cd5d356be8c', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('909e0a03-8c52-4825-85d1-1b6843cb9330', '8dbd4190-e4b7-4eab-a653-d1e78316b57f', '2024-12-23 18:29:06', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('6cce9b7a-35a9-43aa-90f6-fe55c35c4f08', '8dbd4190-e4b7-4eab-a653-d1e78316b57f', '2024-12-25 18:29:06', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('c2ea0ffa-9b6d-4c87-918b-d7da4f87238e', '8dbd4190-e4b7-4eab-a653-d1e78316b57f', '2024-12-27 18:29:06', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('5495effb-0315-4436-aaac-8b4aa6211c33', '8dbd4190-e4b7-4eab-a653-d1e78316b57f', '2024-12-29 18:29:06', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('6735126b-c3b0-456f-96f3-559196b91301', '6e8f98b3-e4c7-498e-a92b-b2927aeb8dd2', '2024-12-31 18:29:06', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('90eb2599-7317-4063-984c-a630487d4206', '6e8f98b3-e4c7-498e-a92b-b2927aeb8dd2', '2024-12-31 18:29:06', 30, 'Added');
                

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241222152647_InitialDataSeed', '8.0.2');

COMMIT;

