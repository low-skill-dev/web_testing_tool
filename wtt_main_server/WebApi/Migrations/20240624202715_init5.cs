using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbUserImapAccount");

            migrationBuilder.DropTable(
                name: "DbUserProxy");

            migrationBuilder.AlterColumn<IPAddress>(
                name: "RegistrationIPAddress",
                table: "Users",
                type: "inet",
                nullable: true,
                oldClrType: typeof(IPAddress),
                oldType: "inet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<IPAddress>(
                name: "RegistrationIPAddress",
                table: "Users",
                type: "inet",
                nullable: false,
                oldClrType: typeof(IPAddress),
                oldType: "inet",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DbUserImapAccount",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConnectionUrl = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    SubscriptionRequired = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbUserImapAccount", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "DbUserProxy",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Changed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uuid", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: false),
                    SubscriptionRequired = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbUserProxy", x => x.Guid);
                });
        }
    }
}
