using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProxies",
                table: "UserProxies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserImapAccounts",
                table: "UserImapAccounts");

            migrationBuilder.RenameTable(
                name: "UserProxies",
                newName: "DbUserProxy");

            migrationBuilder.RenameTable(
                name: "UserImapAccounts",
                newName: "DbUserImapAccount");

            migrationBuilder.AlterColumn<TimeOnly[]>(
                name: "RunTimes",
                table: "TestScenarios",
                type: "time without time zone[]",
                nullable: true,
                oldClrType: typeof(TimeOnly[]),
                oldType: "time without time zone[]");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbUserProxy",
                table: "DbUserProxy",
                column: "Guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbUserImapAccount",
                table: "DbUserImapAccount",
                column: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DbUserProxy",
                table: "DbUserProxy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbUserImapAccount",
                table: "DbUserImapAccount");

            migrationBuilder.RenameTable(
                name: "DbUserProxy",
                newName: "UserProxies");

            migrationBuilder.RenameTable(
                name: "DbUserImapAccount",
                newName: "UserImapAccounts");

            migrationBuilder.AlterColumn<TimeOnly[]>(
                name: "RunTimes",
                table: "TestScenarios",
                type: "time without time zone[]",
                nullable: false,
                defaultValue: new TimeOnly[0],
                oldClrType: typeof(TimeOnly[]),
                oldType: "time without time zone[]",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProxies",
                table: "UserProxies",
                column: "Guid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserImapAccounts",
                table: "UserImapAccounts",
                column: "Guid");
        }
    }
}
