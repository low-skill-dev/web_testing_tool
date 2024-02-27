using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wtt_main_server_api.Migrations
{
    /// <inheritdoc />
    public partial class dev : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ADbObjectWithGuidSequence");

            migrationBuilder.CreateTable(
                name: "DbJwtIdentifier",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    JtiSha512 = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OriginIssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IPAddress = table.Column<IPAddress>(type: "inet", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    RelatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbJwtIdentifier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailSendLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Addressee = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsSucceeded = table.Column<bool>(type: "boolean", nullable: false),
                    RelatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailSendLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImapAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ConnectionUrl = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    SubscriptionRequired = table.Column<int>(type: "integer", nullable: false),
                    RelatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImapAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proxies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionRequired = table.Column<int>(type: "integer", nullable: false),
                    RelatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proxies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestScenarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EnableEmailNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    ActionsJson = table.Column<string>(type: "jsonb", nullable: false),
                    EntryPoint = table.Column<Guid>(type: "uuid", nullable: false),
                    ArgTypes = table.Column<int[]>(type: "integer[]", nullable: false),
                    ArgNames = table.Column<string[]>(type: "text[]", nullable: false),
                    Sha512 = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    RelatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestScenarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserImapAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ConnectionUrl = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    RelatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImapAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProxies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    RelatedUserGuid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProxies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"ADbObjectWithGuidSequence\"')"),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    IsDisabled = table.Column<bool>(type: "boolean", nullable: false),
                    Email = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    EmailConfirmedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: false),
                    PasswordLastChanged = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotpSecretKeyForSha512 = table.Column<byte[]>(type: "bytea", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbJwtIdentifier_Guid",
                table: "DbJwtIdentifier",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailSendLogs_Guid",
                table: "EmailSendLogs",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImapAccounts_Guid",
                table: "ImapAccounts",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proxies_Guid",
                table: "Proxies",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestScenarios_Guid",
                table: "TestScenarios",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserImapAccounts_Guid",
                table: "UserImapAccounts",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProxies_Guid",
                table: "UserProxies",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Guid",
                table: "Users",
                column: "Guid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbJwtIdentifier");

            migrationBuilder.DropTable(
                name: "EmailSendLogs");

            migrationBuilder.DropTable(
                name: "ImapAccounts");

            migrationBuilder.DropTable(
                name: "Proxies");

            migrationBuilder.DropTable(
                name: "TestScenarios");

            migrationBuilder.DropTable(
                name: "UserImapAccounts");

            migrationBuilder.DropTable(
                name: "UserProxies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropSequence(
                name: "ADbObjectWithGuidSequence");
        }
    }
}
