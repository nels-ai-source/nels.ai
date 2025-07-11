using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nels.Aigc.Migrations
{
    /// <inheritdoc />
    public partial class update_version : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "sys_Roles",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "sys_ClaimTypes",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "sys_BackgroundJobs",
                type: "character varying(96)",
                maxLength: 96,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "sys_Roles");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "sys_ClaimTypes");

            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "sys_BackgroundJobs");
        }
    }
}
