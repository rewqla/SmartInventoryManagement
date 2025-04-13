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

