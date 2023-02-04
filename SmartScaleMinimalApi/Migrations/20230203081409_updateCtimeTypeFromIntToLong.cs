using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartScaleMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class updateCtimeTypeFromIntToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Glass",
                table: "PersonInfos");

            migrationBuilder.DropColumn(
                name: "Hat",
                table: "PersonInfos");

            migrationBuilder.AlterColumn<long>(
                name: "Ctime",
                table: "Scales",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Ctime",
                table: "PersonInfos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Ctime",
                table: "FaceEigens",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Ctime",
                table: "Scales",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Ctime",
                table: "PersonInfos",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "Glass",
                table: "PersonInfos",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Hat",
                table: "PersonInfos",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ctime",
                table: "FaceEigens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
