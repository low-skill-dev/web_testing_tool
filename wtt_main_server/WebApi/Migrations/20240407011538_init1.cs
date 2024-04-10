using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "EmailSendLogs",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Addressee = table.Column<string>(type: "text", nullable: false),
                    IsSucceeded = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSendLogs", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "RecoveryJwts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    JtiSha512 = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginIssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IPAddress = table.Column<IPAddress>(type: "inet", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecoveryJwts", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "RefreshJwts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    JtiSha512 = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginIssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IPAddress = table.Column<IPAddress>(type: "inet", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshJwts", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioRuns",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    ScenarioGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    Started = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Completed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsSucceeded = table.Column<bool>(type: "boolean", nullable: true),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    ProcessorTime = table.Column<TimeSpan>(type: "interval", nullable: true),
                    RunReason = table.Column<int>(type: "integer", nullable: true),
                    ScenarioJsonSnapshot = table.Column<string>(type: "jsonb", nullable: true),
                    ScenarioJsonResult = table.Column<string>(type: "jsonb", nullable: true),
                    InputValues = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    OutputValues = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioRuns", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "TestScenarios",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    EnableEmailNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    ActionsJson = table.Column<string>(type: "jsonb", nullable: false),
                    EntryPoint = table.Column<Guid>(type: "uuid", nullable: false),
                    ArgTypes = table.Column<int[]>(type: "integer[]", nullable: false),
                    ArgNames = table.Column<string[]>(type: "text[]", nullable: false),
                    RunIntervalMinutes = table.Column<int>(type: "integer", nullable: true),
                    RunTimes = table.Column<TimeOnly[]>(type: "time without time zone[]", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestScenarios", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "UserImapAccounts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ConnectionUrl = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    SubscriptionRequired = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImapAccounts", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "UserProxies",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    SubscriptionRequired = table.Column<int>(type: "integer", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProxies", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    RegistrationIPAddress = table.Column<IPAddress>(type: "inet", nullable: false),
                    RegistrationCountry = table.Column<string>(type: "text", nullable: true),
                    RegistrationCity = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    EmailConfirmedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    PasswordLastChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotpSecretKeyForSha512 = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailSendLogs");

            migrationBuilder.DropTable(
                name: "RecoveryJwts");

            migrationBuilder.DropTable(
                name: "RefreshJwts");

            migrationBuilder.DropTable(
                name: "ScenarioRuns");

            migrationBuilder.DropTable(
                name: "TestScenarios");

            migrationBuilder.DropTable(
                name: "UserImapAccounts");

            migrationBuilder.DropTable(
                name: "UserProxies");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
