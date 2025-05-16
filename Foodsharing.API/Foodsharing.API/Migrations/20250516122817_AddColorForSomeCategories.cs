using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodsharing.API.Migrations
{
    /// <inheritdoc />
    public partial class AddColorForSomeCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("97ca5649-c73a-4a21-9842-b6fb8bd9dcda"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                column: "Color",
                value: "#FFE699");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1141deac-fddb-43c5-85e3-6c5b6d7ec314"),
                column: "Color",
                value: "#F5F5DC");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2d740f5a-173e-4f1b-974d-c007a2256c37"),
                column: "Color",
                value: "#FFF0E6");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3a45831f-89fa-40ce-b236-35adcde88d56"),
                column: "Color",
                value: "#FFC299");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("42139ca4-f9fc-41b9-8a89-0a8518177279"),
                column: "Color",
                value: "#FFF0F5");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                column: "Color",
                value: "#FAF0DC");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                column: "Color",
                value: "#E6FFE6");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("863bbb48-1b44-4f05-a380-8995de42b86f"),
                column: "Color",
                value: "#FFC299");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b3d5fb72-7622-4c35-be37-c537d873e640"),
                column: "Color",
                value: "#E6F7FF");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c09a6c67-bed1-479c-8a44-6a674ebb2bfa"),
                column: "Color",
                value: "#FFC299");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1141deac-fddb-43c5-85e3-6c5b6d7ec314"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("2d740f5a-173e-4f1b-974d-c007a2256c37"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3a45831f-89fa-40ce-b236-35adcde88d56"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("42139ca4-f9fc-41b9-8a89-0a8518177279"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("863bbb48-1b44-4f05-a380-8995de42b86f"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("b3d5fb72-7622-4c35-be37-c537d873e640"),
                column: "Color",
                value: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c09a6c67-bed1-479c-8a44-6a674ebb2bfa"),
                column: "Color",
                value: "");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "Name", "ParentId" },
                values: new object[] { new Guid("97ca5649-c73a-4a21-9842-b6fb8bd9dcda"), "", "Напитки", new Guid("b3d5fb72-7622-4c35-be37-c537d873e640") });
        }
    }
}
