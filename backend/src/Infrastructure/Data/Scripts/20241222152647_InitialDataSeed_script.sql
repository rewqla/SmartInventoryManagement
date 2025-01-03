START TRANSACTION;

INSERT INTO "Roles" ("Id", "Name") VALUES ('942b0903-b2c2-47a8-8d55-509de9ab57bf','Admin')

INSERT INTO "Roles" ("Id", "Name") VALUES ('713b6b52-7b8e-43b4-b42a-2fa5f8de9ae7','Manager')

INSERT INTO "Roles" ("Id", "Name") VALUES ('7769a8b1-8362-49f8-bef1-4a8b2407d73d','Worker')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('427cb69f-cc0a-4fb4-ae82-639c52f4494d', 'Admin User', '1234567890', 'admin@example.com', '50C0EEBDF383F27EBC8ED521C6940C00A8A86E3B7CE61E917ECCCDB4E1C5ABDD-5B4E524CD7375C849C2BD7AF81C346BD', '942b0903-b2c2-47a8-8d55-509de9ab57bf')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('75cd8a90-944e-4ce5-9a1b-af27b762f032', 'Manager User', '0987654321', 'manager@example.com', '8A4C7BF64C44B02EF21EE00750D8AF13EB17BEB8AB11B87D8844034ED7309A21-9BAEDF4B0A17BD61E776CB0AB9474D03', '713b6b52-7b8e-43b4-b42a-2fa5f8de9ae7')

INSERT INTO "Users" ("Id", "Name", "Phone", "Email", "PasswordHash", "RoleId") VALUES ('749df263-46d3-4a4a-b4c2-e0ad338d263d', 'Worker User', '1112223333', 'worker@example.com', '990A537798C702A5A75E718CECD3A6853815A814D21B9F772AAD89D7BD3FE92C-174AAF807943584060A887E417661AD6', '7769a8b1-8362-49f8-bef1-4a8b2407d73d')

INSERT INTO "Categories" ("Id", "Name") VALUES ('643993b0-b220-4c89-bee1-2e93d98f1d2e', 'Electronics')

INSERT INTO "Categories" ("Id", "Name") VALUES ('f8630e86-a50e-4c77-a6e2-a86def207f83', 'Furniture')

INSERT INTO "Categories" ("Id", "Name") VALUES ('7756c7b5-d5a4-466f-bfd5-67fc509e6d8c', 'Clothing')

INSERT INTO "Categories" ("Id", "Name") VALUES ('caa2e344-f9a2-43d6-b8e8-08eab40df362', 'Sports')

INSERT INTO "Categories" ("Id", "Name") VALUES ('04416cca-1095-47eb-b59d-226166eb3734', 'Beauty')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('b5bbe294-b95f-4602-a9cf-0963df15f508', 'Central Warehouse','Rivne')

INSERT INTO "Warehouses" ("Id", "Name", "Location") VALUES ('fa95dc16-a90a-4de4-ab0c-87243d95cecc', 'Secondary Warehouse', 'Zhytomyr')


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('35f33416-5c5f-42fa-881e-fc5182ffb07a', 'Smartphone', 'ELEC-001', 'Latest smartphone', 699.99, '643993b0-b220-4c89-bee1-2e93d98f1d2e');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('22a5c824-f101-4c02-9f43-70c90a449605', 'Laptop', 'ELEC-002', 'High-performance laptop', 1299.99, '643993b0-b220-4c89-bee1-2e93d98f1d2e');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('446dc68d-35d2-4c06-807b-9d244e183341', 'Office Chair', 'FURN-001', 'Ergonomic office chair', 149.99, 'f8630e86-a50e-4c77-a6e2-a86def207f83');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('c3e48cf5-949e-4fd5-9b38-01b1b51c9673', 'Sofa', 'FURN-002', 'Comfortable sofa', 499.99, 'f8630e86-a50e-4c77-a6e2-a86def207f83');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('1f51d361-1b76-481f-8487-a5c9a2bdd20c', 'T-Shirt', 'CLOT-001', '100% cotton T-shirt', 19.99, '7756c7b5-d5a4-466f-bfd5-67fc509e6d8c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('de018b1c-dc47-49fb-9f8d-14e6d1d36434', 'Jeans', 'CLOT-002', 'Stylish jeans', 49.99, '7756c7b5-d5a4-466f-bfd5-67fc509e6d8c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('b7f7e395-fbdb-4975-b2bf-29792fad77eb', 'Football', 'SPRT-001', 'Professional football', 29.99, 'caa2e344-f9a2-43d6-b8e8-08eab40df362');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('673d96c6-5455-4280-93f4-06f887923edd', 'Basketball', 'SPRT-002', 'Durable basketball', 39.99, 'caa2e344-f9a2-43d6-b8e8-08eab40df362');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('38b63207-c0d4-4cfd-94a5-67122c732261', 'Lipstick', 'BEAU-001', 'Matte lipstick', 9.99, '04416cca-1095-47eb-b59d-226166eb3734');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('7e548275-da07-4ce3-8914-99cf93dd756c', 'Perfume', 'BEAU-002', 'Luxury perfume', 99.99, '04416cca-1095-47eb-b59d-226166eb3734');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('de9625e3-6c89-430c-a418-4efef72a1904', 'Tablet', 'ELEC-003', 'Portable tablet', 499.99, '643993b0-b220-4c89-bee1-2e93d98f1d2e');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('cc281990-c212-4893-8ac6-f041bd6506e0', 'Headphones', 'ELEC-004', 'Noise-cancelling headphones', 199.99, '643993b0-b220-4c89-bee1-2e93d98f1d2e');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('d6089812-337b-4cfc-81b5-90c7fbc84450', 'Desk', 'FURN-003', 'Wooden office desk', 299.99, 'f8630e86-a50e-4c77-a6e2-a86def207f83');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('78ed1b23-146b-4c86-ab60-e285165bfab5', 'Bookshelf', 'FURN-004', '5-shelf bookshelf', 129.99, 'f8630e86-a50e-4c77-a6e2-a86def207f83');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('4136e2e5-149f-4814-90ec-12a16047c314', 'Jacket', 'CLOT-003', 'Waterproof jacket', 89.99, '7756c7b5-d5a4-466f-bfd5-67fc509e6d8c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('f4de0848-a2cb-4eda-816b-28f2dc2e18f6', 'Sneakers', 'CLOT-004', 'Comfortable sneakers', 79.99, '7756c7b5-d5a4-466f-bfd5-67fc509e6d8c');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('26e21dbe-0edb-416b-a20c-5197af98e684', 'Tennis Racket', 'SPRT-003', 'Lightweight tennis racket', 59.99, 'caa2e344-f9a2-43d6-b8e8-08eab40df362');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('947a86a8-5c8a-4a0f-bb30-f036ea91f672', 'Yoga Mat', 'SPRT-004', 'Non-slip yoga mat', 24.99, 'caa2e344-f9a2-43d6-b8e8-08eab40df362');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('7042f620-6d62-4c35-b040-4d6619052aa9', 'Face Cream', 'BEAU-003', 'Anti-aging face cream', 39.99, '04416cca-1095-47eb-b59d-226166eb3734');
                


                    INSERT INTO "Products" ("Id", "Name", "SKU", "Description", "UnitPrice", "CategoryId")
                    VALUES ('fdf4294e-144f-48ab-8b02-2be347d9d6bd', 'Hair Dryer', 'BEAU-004', 'High-speed hair dryer', 79.99, '04416cca-1095-47eb-b59d-226166eb3734');
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('396666ac-844e-458b-931c-6785e6cf46be', '35f33416-5c5f-42fa-881e-fc5182ffb07a', 'b5bbe294-b95f-4602-a9cf-0963df15f508', 100);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('95707fb5-38a4-45c7-823b-0b456956d65f', '446dc68d-35d2-4c06-807b-9d244e183341', 'b5bbe294-b95f-4602-a9cf-0963df15f508', 50);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('60c5ba53-8f44-4977-b10a-00dbbb4ea23d', '1f51d361-1b76-481f-8487-a5c9a2bdd20c', 'b5bbe294-b95f-4602-a9cf-0963df15f508', 200);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('a0bbcbc9-e738-47f7-8868-918786ae594d', 'b7f7e395-fbdb-4975-b2bf-29792fad77eb', 'b5bbe294-b95f-4602-a9cf-0963df15f508', 150);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('291f75ea-0b50-4d50-b081-b7c15fa1289c', '7e548275-da07-4ce3-8914-99cf93dd756c', 'b5bbe294-b95f-4602-a9cf-0963df15f508', 75);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('01acd213-601f-44af-b044-3c07874d3ada', '35f33416-5c5f-42fa-881e-fc5182ffb07a', 'fa95dc16-a90a-4de4-ab0c-87243d95cecc', 120);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('82ffc01f-730b-40b1-ba4f-590bb4b1a11e', '446dc68d-35d2-4c06-807b-9d244e183341', 'fa95dc16-a90a-4de4-ab0c-87243d95cecc', 60);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('72988444-d1e5-470f-b34f-e1117be316b8', '1f51d361-1b76-481f-8487-a5c9a2bdd20c', 'fa95dc16-a90a-4de4-ab0c-87243d95cecc', 180);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('15e8376e-dc3d-4116-8d53-7a07e1ba4ed3', 'b7f7e395-fbdb-4975-b2bf-29792fad77eb', 'fa95dc16-a90a-4de4-ab0c-87243d95cecc', 130);
                


                    INSERT INTO "Inventories" ("Id", "ProductId", "WarehouseId", "Quantity")
                    VALUES ('13981afe-688d-4f98-b5bc-857dd552d119', '7e548275-da07-4ce3-8914-99cf93dd756c', 'fa95dc16-a90a-4de4-ab0c-87243d95cecc', 85);
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('a4791123-e061-4c4e-8358-9f026af59263', '396666ac-844e-458b-931c-6785e6cf46be', '2024-12-24 18:40:38', 20, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('8ed969be-246a-4180-9e2b-ad60edcc9e4c', '396666ac-844e-458b-931c-6785e6cf46be', '2024-12-26 18:40:38', -5, 'Adjusted');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('6609b1bf-e643-4515-b17b-fdfbfd9e7884', '396666ac-844e-458b-931c-6785e6cf46be', '2024-12-28 18:40:38', 15, 'Released');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('a70a151d-c629-4d06-bdfe-1b785816467a', '396666ac-844e-458b-931c-6785e6cf46be', '2024-12-30 18:40:38', -10, 'Removed');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('f729dda5-3c6c-4772-bc00-dd6210cb857c', '01acd213-601f-44af-b044-3c07874d3ada', '2025-01-01 18:40:38', 30, 'Added');
                


                    INSERT INTO "InventoryLogs" ("Id", "InventoryId", "Timestamp", "QuantityChanged", "ChangeType")
                    VALUES ('fc5bc6b6-1dbf-4184-b436-9bf027246e34', '01acd213-601f-44af-b044-3c07874d3ada', '2025-01-01 18:40:38', 30, 'Added');
                

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241222152647_InitialDataSeed', '8.0.2');

COMMIT;

