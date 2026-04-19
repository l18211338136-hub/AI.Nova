using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AI.Nova.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderStatusEnumComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"));

            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"ProductShortId\"')",
                comment: "用于生成友好 URL 的短整型 ID",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldDefaultValueSql: "nextval('\"ProductShortId\"')",
                oldComment: "用于生成友好 URL 的短整型 ID");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                nullable: true,
                comment: "0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "当前订单所处的业务状态枚举值");

            migrationBuilder.RestartSequence(
                name: "ProductShortId",
                startValue: 10300L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ShortId",
                table: "Products",
                type: "integer",
                nullable: true,
                defaultValueSql: "nextval('\"ProductShortId\"')",
                comment: "用于生成友好 URL 的短整型 ID",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValueSql: "nextval('\"ProductShortId\"')",
                oldComment: "用于生成友好 URL 的短整型 ID");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Orders",
                type: "smallint",
                nullable: true,
                comment: "当前订单所处的业务状态枚举值",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true,
                oldComment: "0:待付款, 1:已付款, 2:已发货, 3:已完成, 4:已取消, 5:退款中");

            migrationBuilder.RestartSequence(
                name: "ProductShortId",
                startValue: 10051L);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "ConcurrencyStamp", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "ElevatedAccessTokenRequestedOn", "Email", "EmailConfirmed", "EmailTokenRequestedOn", "FullName", "Gender", "HasProfilePicture", "IsDeleted", "LockoutEnabled", "LockoutEnd", "ModifiedBy", "ModifiedOn", "NormalizedEmail", "NormalizedUserName", "OtpRequestedOn", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhoneNumberTokenRequestedOn", "ResetPasswordTokenRequestedOn", "SecurityStamp", "TwoFactorEnabled", "TwoFactorTokenRequestedOn", "UserName" },
                values: new object[] { new Guid("8ff71671-a1d6-4f97-abb9-d87d7b47d6e7"), 0, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "315e1a26-5b3a-4544-8e91-2760cd28e231", null, null, null, null, null, "761516331@qq.com", true, new DateTimeOffset(new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "AI.Nova test account", 0, null, null, true, null, null, null, "761516331@QQ.COM", "TEST", null, "AQAAAAIAAYagAAAAEP0v3wxkdWtMkHA3Pp5/JfS+42/Qto9G05p2mta6dncSK37hPxEHa3PGE4aqN30Aag==", "+31684207362", true, null, null, "959ff4a9-4b07-4cc1-8141-c5fc033daf83", false, null, "test" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "'Email' IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PhoneNumber",
                table: "Users",
                column: "PhoneNumber",
                unique: true,
                filter: "'PhoneNumber' IS NOT NULL");
        }
    }
}
