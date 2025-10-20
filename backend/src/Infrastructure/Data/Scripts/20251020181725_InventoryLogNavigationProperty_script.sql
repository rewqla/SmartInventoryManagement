START TRANSACTION;

ALTER TABLE public."InventoryLogs" ADD "ChangedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

CREATE INDEX "IX_InventoryLogs_ChangedById" ON public."InventoryLogs" ("ChangedById");

ALTER TABLE public."InventoryLogs" ADD CONSTRAINT "FK_InventoryLogs_Users_ChangedById" FOREIGN KEY ("ChangedById") REFERENCES public."Users" ("Id") ON DELETE CASCADE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251020181725_InventoryLogNavigationProperty', '8.0.2');

COMMIT;

