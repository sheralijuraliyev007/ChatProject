using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Age", "Bio", "Firstname", "Gender", "Lastname", "PasswordHash", "PhotoData", "Role", "Username" },
                values: new object[] { new Guid("78f0f38f-78df-4617-be07-27ad653b8983"), null, null, "Admin", "Male", "Adminov", "AQAAAAIAAYagAAAAEIToa1sd6SlMZXJUWV5bpoPpTQEhrjzcP6n3925c7maUDSZ9IGg2XRw3TtIEIH1YMg==", null, "admin", "admin007" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("78f0f38f-78df-4617-be07-27ad653b8983"));
        }
    }
}
