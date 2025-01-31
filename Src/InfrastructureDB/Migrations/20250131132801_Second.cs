using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InfrastructureDB.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                schema: "kurier",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 31, 17, 28, 1, 234, DateTimeKind.Local).AddTicks(7294),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2025, 1, 31, 17, 21, 40, 46, DateTimeKind.Local).AddTicks(2571));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                schema: "kurier",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 31, 17, 28, 1, 232, DateTimeKind.Local).AddTicks(2250),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2025, 1, 31, 17, 21, 40, 43, DateTimeKind.Local).AddTicks(9442));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdate",
                schema: "kurier",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 31, 17, 21, 40, 46, DateTimeKind.Local).AddTicks(2571),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2025, 1, 31, 17, 28, 1, 234, DateTimeKind.Local).AddTicks(7294));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                schema: "kurier",
                table: "Orders",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2025, 1, 31, 17, 21, 40, 43, DateTimeKind.Local).AddTicks(9442),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2025, 1, 31, 17, 28, 1, 232, DateTimeKind.Local).AddTicks(2250));
        }
    }
}
