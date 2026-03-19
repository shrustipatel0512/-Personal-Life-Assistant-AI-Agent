$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path -Parent $PSScriptRoot
$appSettingsPath = Join-Path $repoRoot 'src\PersonalLifeAssistant.Api\appsettings.json'
$npgsqlPath = Join-Path $repoRoot 'src\PersonalLifeAssistant.Api\bin\Debug\net8.0\Npgsql.dll'

if (-not (Test-Path $appSettingsPath)) {
  throw "Could not find appsettings.json at $appSettingsPath"
}

if (-not (Test-Path $npgsqlPath)) {
  throw "Could not find Npgsql.dll at $npgsqlPath"
}

Add-Type -Path $npgsqlPath

$appSettings = Get-Content $appSettingsPath -Raw | ConvertFrom-Json
$connectionString = $appSettings.ConnectionStrings.DefaultConnection

if ([string]::IsNullOrWhiteSpace($connectionString)) {
  throw 'DefaultConnection is missing from appsettings.json'
}

$builder = [Npgsql.NpgsqlConnectionStringBuilder]::new($connectionString)
$targetDatabase = $builder.Database

$adminBuilder = [Npgsql.NpgsqlConnectionStringBuilder]::new($connectionString)
$adminBuilder.Database = 'postgres'

$schemaSql = @"
CREATE TABLE IF NOT EXISTS users (
    "Id" uuid PRIMARY KEY,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "UpdatedAtUtc" timestamp with time zone NULL,
    "FullName" character varying(200) NOT NULL,
    "Email" character varying(320) NOT NULL,
    "PasswordHash" character varying(512) NOT NULL,
    "TimeZone" character varying(100) NOT NULL,
    "Role" character varying(50) NOT NULL,
    "PreferencesJson" text NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_users_Email" ON users ("Email");

CREATE TABLE IF NOT EXISTS chat_history (
    "Id" uuid PRIMARY KEY,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "UpdatedAtUtc" timestamp with time zone NULL,
    "UserId" uuid NOT NULL REFERENCES users("Id") ON DELETE CASCADE,
    "Role" character varying(30) NOT NULL,
    "Message" character varying(8000) NOT NULL,
    "Intent" character varying(100) NOT NULL,
    "ContextSnapshotJson" text NOT NULL
);

CREATE INDEX IF NOT EXISTS "IX_chat_history_UserId" ON chat_history ("UserId");

CREATE TABLE IF NOT EXISTS expenses (
    "Id" uuid PRIMARY KEY,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "UpdatedAtUtc" timestamp with time zone NULL,
    "UserId" uuid NOT NULL REFERENCES users("Id") ON DELETE CASCADE,
    "Amount" numeric NOT NULL,
    "Currency" character varying(10) NOT NULL,
    "Category" character varying(120) NOT NULL,
    "OccurredOnUtc" timestamp with time zone NOT NULL,
    "Notes" text NULL
);

CREATE INDEX IF NOT EXISTS "IX_expenses_UserId" ON expenses ("UserId");

CREATE TABLE IF NOT EXISTS habits (
    "Id" uuid PRIMARY KEY,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "UpdatedAtUtc" timestamp with time zone NULL,
    "UserId" uuid NOT NULL REFERENCES users("Id") ON DELETE CASCADE,
    "Name" character varying(120) NOT NULL,
    "Frequency" character varying(50) NOT NULL,
    "TargetValue" integer NOT NULL,
    "CurrentValue" integer NOT NULL,
    "Unit" character varying(30) NOT NULL,
    "AiSuggestion" text NOT NULL
);

CREATE INDEX IF NOT EXISTS "IX_habits_UserId" ON habits ("UserId");

CREATE TABLE IF NOT EXISTS notifications (
    "Id" uuid PRIMARY KEY,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "UpdatedAtUtc" timestamp with time zone NULL,
    "UserId" uuid NOT NULL REFERENCES users("Id") ON DELETE CASCADE,
    "Channel" character varying(30) NOT NULL,
    "Message" character varying(1000) NOT NULL,
    "ScheduledForUtc" timestamp with time zone NOT NULL,
    "IsSent" boolean NOT NULL,
    "MetadataJson" text NOT NULL
);

CREATE INDEX IF NOT EXISTS "IX_notifications_UserId" ON notifications ("UserId");

CREATE TABLE IF NOT EXISTS tasks (
    "Id" uuid PRIMARY KEY,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "UpdatedAtUtc" timestamp with time zone NULL,
    "UserId" uuid NOT NULL REFERENCES users("Id") ON DELETE CASCADE,
    "Title" character varying(200) NOT NULL,
    "Description" character varying(2000) NULL,
    "Category" integer NOT NULL,
    "Priority" integer NOT NULL,
    "IsCompleted" boolean NOT NULL,
    "DueDateUtc" timestamp with time zone NULL,
    "EstimatedMinutes" integer NOT NULL,
    "AiReasoning" character varying(4000) NOT NULL
);

CREATE INDEX IF NOT EXISTS "IX_tasks_UserId" ON tasks ("UserId");

CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
SELECT '20260319120000_InitialCreate', '8.0.4'
WHERE NOT EXISTS (
    SELECT 1
    FROM "__EFMigrationsHistory"
    WHERE "MigrationId" = '20260319120000_InitialCreate'
);
"@

function Invoke-NonQuery([string]$connString, [string]$sql) {
  $connection = [Npgsql.NpgsqlConnection]::new($connString)
  try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = $sql
    [void]$command.ExecuteNonQuery()
  }
  finally {
    $connection.Dispose()
  }
}

$adminConnection = [Npgsql.NpgsqlConnection]::new($adminBuilder.ConnectionString)
try {
  $adminConnection.Open()
  $existsCommand = $adminConnection.CreateCommand()
  $existsCommand.CommandText = 'SELECT 1 FROM pg_database WHERE datname = @databaseName'
  [void]$existsCommand.Parameters.AddWithValue('databaseName', $targetDatabase)
  $databaseExists = $existsCommand.ExecuteScalar()

  if (-not $databaseExists) {
    $createCommand = $adminConnection.CreateCommand()
    $createCommand.CommandText = "CREATE DATABASE ""$targetDatabase"""
    [void]$createCommand.ExecuteNonQuery()
  }
}
finally {
  $adminConnection.Dispose()
}

Invoke-NonQuery -connString $builder.ConnectionString -sql $schemaSql
Write-Host "Database '$targetDatabase' updated successfully."
