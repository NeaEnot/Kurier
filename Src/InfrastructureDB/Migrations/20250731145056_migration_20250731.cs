using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InfrastructureDB.Migrations
{
    /// <inheritdoc />
    public partial class migration_20250731 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "kurier",
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Permissions" },
                values: new object[,]
                {
                    { new Guid("0af2685a-16d7-4842-9d40-397e9166dd53"), "adam.nick@kurier.ru", "Адам Николаев", "ds;ljkfhatfgil34", 24 },
                    { new Guid("224d30a8-c2a1-4c5e-b84d-0e018ce67a7d"), "nikolaeva.d.d@yandex.ru", "Дарина Николаева", "kmvw8234r", 7 },
                    { new Guid("28c02b6f-a4fd-41f2-b0e9-a0bfa0bbf7af"), "ilya_bog@mail.ru", "Илья Богданов", ",lsdafjk0982", 7 },
                    { new Guid("5b09eec7-452b-4b0d-875f-472d8f3babd1"), "ekat.grig@kurier.ru", "Екатерина Григорьева", "5tgbwdsaf093", 4064 },
                    { new Guid("9a78b69e-8049-4daf-9a99-8398b58791ef"), "matveytokarev@kurier.ru", "Матвей Токарев", "pikjsr7e8ftg2w4r", 24 },
                    { new Guid("bf36603a-5b14-4b1b-bae4-e9a59c8d9dbd"), "maxxmirno@google.com", "Максим Смирнов", "mbge089yw=", 7 }
                });

            migrationBuilder.InsertData(
                schema: "kurier",
                table: "Orders",
                columns: new[] { "Id", "Created", "DeliveryAddress", "DepartureAddress", "LastUpdate", "UserId", "Weight" },
                values: new object[,]
                {
                    { new Guid("56a302e2-675a-45ab-ae0e-535e2f36ee0f"), new DateTime(2025, 2, 7, 20, 35, 10, 489, DateTimeKind.Local).AddTicks(6107), "пр. 1905 года, д. 92, кв. 83", "шоссе Ленина, д. 93, кв. 10", new DateTime(2025, 2, 7, 20, 35, 10, 490, DateTimeKind.Local).AddTicks(8626), new Guid("224d30a8-c2a1-4c5e-b84d-0e018ce67a7d"), 4.0m },
                    { new Guid("f8e666d8-0f7f-4c56-b575-d5fc30bfeb39"), new DateTime(2025, 2, 7, 20, 35, 10, 490, DateTimeKind.Local).AddTicks(9974), "наб. Бухарестская, д. 11, кв. 35", "пл. Сталина, д. 62, кв. 49", new DateTime(2025, 2, 7, 20, 35, 10, 490, DateTimeKind.Local).AddTicks(9977), new Guid("bf36603a-5b14-4b1b-bae4-e9a59c8d9dbd"), 10.0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("56a302e2-675a-45ab-ae0e-535e2f36ee0f"));

            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("f8e666d8-0f7f-4c56-b575-d5fc30bfeb39"));

            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0af2685a-16d7-4842-9d40-397e9166dd53"));

            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("28c02b6f-a4fd-41f2-b0e9-a0bfa0bbf7af"));

            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5b09eec7-452b-4b0d-875f-472d8f3babd1"));

            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("9a78b69e-8049-4daf-9a99-8398b58791ef"));

            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("224d30a8-c2a1-4c5e-b84d-0e018ce67a7d"));

            migrationBuilder.DeleteData(
                schema: "kurier",
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("bf36603a-5b14-4b1b-bae4-e9a59c8d9dbd"));
        }
    }
}
