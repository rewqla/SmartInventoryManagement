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

