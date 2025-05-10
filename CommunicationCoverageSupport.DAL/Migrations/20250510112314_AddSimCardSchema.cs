using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommunicationCoverageSupport.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSimCardSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accs",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Artworks",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artworks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SimCards",
                columns: table => new
                {
                    Iccid = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Imsi = table.Column<long>(type: "bigint", nullable: false),
                    Msisdn = table.Column<long>(type: "bigint", nullable: false),
                    Produced = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Installed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Purged = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Kl1 = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pin1 = table.Column<short>(type: "smallint", nullable: false),
                    Pin2 = table.Column<short>(type: "smallint", nullable: false),
                    Puk1 = table.Column<int>(type: "int", nullable: false),
                    Puk2 = table.Column<int>(type: "int", nullable: false),
                    Adm1 = table.Column<long>(type: "bigint", nullable: false),
                    ArtworkId = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    AccId = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    CardOwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimCards", x => x.Iccid);
                    table.ForeignKey(
                        name: "FK_SimCards_Accs_AccId",
                        column: x => x.AccId,
                        principalTable: "Accs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimCards_Artworks_ArtworkId",
                        column: x => x.ArtworkId,
                        principalTable: "Artworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimCards_Users_CardOwnerId",
                        column: x => x.CardOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SimCards_AccId",
                table: "SimCards",
                column: "AccId");

            migrationBuilder.CreateIndex(
                name: "IX_SimCards_ArtworkId",
                table: "SimCards",
                column: "ArtworkId");

            migrationBuilder.CreateIndex(
                name: "IX_SimCards_CardOwnerId",
                table: "SimCards",
                column: "CardOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimCards");

            migrationBuilder.DropTable(
                name: "Accs");

            migrationBuilder.DropTable(
                name: "Artworks");
        }
    }
}
