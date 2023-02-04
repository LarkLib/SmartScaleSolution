using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartScaleMinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFaceImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FaceImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    GatewayId = table.Column<string>(type: "text", nullable: false),
                    FaceNo = table.Column<string>(type: "text", nullable: false),
                    ImageType = table.Column<string>(type: "text", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    Ctime = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaceImages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FaceImages");
        }
    }
}
