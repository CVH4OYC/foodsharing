using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Foodsharing.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeCategories2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                column: "Color",
                value: "#FFF6CC");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3a45831f-89fa-40ce-b236-35adcde88d56"),
                column: "Color",
                value: "#FFD1B3");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                column: "Name",
                value: "Крупы");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("863bbb48-1b44-4f05-a380-8995de42b86f"),
                column: "Color",
                value: "#FFD1B3");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c09a6c67-bed1-479c-8a44-6a674ebb2bfa"),
                column: "Color",
                value: "#FFD1B3");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "Name", "ParentId" },
                values: new object[,]
                {
                    { new Guid("0ed5ccca-722e-45d5-aef0-bb195c59a710"), "#C2E0C2", "Каши", new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b") },
                    { new Guid("1e4f8de2-96c4-4901-9bb0-4bc9407db53b"), "#E6D5B8", "Овсянка", new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4") },
                    { new Guid("4de9dca2-2b6a-4f3c-9c02-56793ac3a222"), "#C2E0C2", "Выпечка", new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b") },
                    { new Guid("5b55a875-9621-47a2-9cf9-187589a3a9b3"), "#FFE699", "Орехи", new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d") },
                    { new Guid("93f17e2f-6e53-4034-9d48-bf4cb12dbdf7"), "#FFE699", "Чипсы", new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d") },
                    { new Guid("a8e43a0e-33fc-4d91-9c84-61e6c0ae0dc5"), "#E6D5B8", "Рис", new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4") },
                    { new Guid("bdd7ad45-2e8a-40b7-a712-0fafea40c718"), "#C2E0C2", "Супы", new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b") },
                    { new Guid("c5f5b7e3-7e24-4a2d-9af1-94c08bd26db5"), "#C2E0C2", "Салаты", new Guid("7e035d5b-ec79-47a7-9b52-d287da24737b") },
                    { new Guid("cf1fef74-cf50-49e4-b32e-b18c9e4c4567"), "#E6D5B8", "Гречка", new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4") },
                    { new Guid("d1c3ab45-1997-4fc2-8ae4-278b5a19f46d"), "#FFE699", "Сухарики", new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("0ed5ccca-722e-45d5-aef0-bb195c59a710"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("1e4f8de2-96c4-4901-9bb0-4bc9407db53b"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("4de9dca2-2b6a-4f3c-9c02-56793ac3a222"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5b55a875-9621-47a2-9cf9-187589a3a9b3"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("93f17e2f-6e53-4034-9d48-bf4cb12dbdf7"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("a8e43a0e-33fc-4d91-9c84-61e6c0ae0dc5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bdd7ad45-2e8a-40b7-a712-0fafea40c718"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c5f5b7e3-7e24-4a2d-9af1-94c08bd26db5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cf1fef74-cf50-49e4-b32e-b18c9e4c4567"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d1c3ab45-1997-4fc2-8ae4-278b5a19f46d"));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("01917a66-9afc-4261-8a94-8cdf29d3256d"),
                column: "Color",
                value: "#FFE699");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("3a45831f-89fa-40ce-b236-35adcde88d56"),
                column: "Color",
                value: "#FFC299");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("5a7e69c0-8a42-4f21-ad5d-f793543369d4"),
                column: "Name",
                value: "Крупа");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("863bbb48-1b44-4f05-a380-8995de42b86f"),
                column: "Color",
                value: "#FFC299");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c09a6c67-bed1-479c-8a44-6a674ebb2bfa"),
                column: "Color",
                value: "#FFC299");
        }
    }
}
