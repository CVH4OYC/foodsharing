using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodsharing.API.Migrations
{
    /// <inheritdoc />
    public partial class changeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PartnershipApplicationStatuses",
                keyColumn: "Id",
                keyValue: new Guid("b8c07b15-c703-40ab-aa09-c3677108f8ea"),
                column: "Name",
                value: "Рассмотрено");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PartnershipApplicationStatuses",
                keyColumn: "Id",
                keyValue: new Guid("b8c07b15-c703-40ab-aa09-c3677108f8ea"),
                column: "Name",
                value: "Рассмотренно");
        }
    }
}
