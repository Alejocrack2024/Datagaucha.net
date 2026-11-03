using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datagaucha.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddBanExpiresAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiresAt",
                table: "Bans",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                table: "Bans");
        }
    }
}
