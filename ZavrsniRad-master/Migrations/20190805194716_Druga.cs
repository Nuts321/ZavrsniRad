using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PolovniAutomobiliZavrsniRad.Migrations
{
    public partial class Druga : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marka",
                columns: table => new
                {
                    MarkaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marka", x => x.MarkaId);
                });

            migrationBuilder.CreateTable(
                name: "TipVozila",
                columns: table => new
                {
                    TipVozilaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipVozila", x => x.TipVozilaId);
                });

            migrationBuilder.CreateTable(
                name: "Modeli",
                columns: table => new
                {
                    ModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MarkaId = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Modeli__E8D7A12CCDD27570", x => x.ModelId);
                    table.ForeignKey(
                        name: "FK__Modeli__MarkaId__4BAC3F29",
                        column: x => x.MarkaId,
                        principalTable: "Marka",
                        principalColumn: "MarkaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vozilo",
                columns: table => new
                {
                    VoziloId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MarkaId = table.Column<int>(nullable: false),
                    ModelId = table.Column<int>(nullable: false),
                    TipVozilaId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<string>(maxLength: 450, nullable: false),
                    Kubikaza = table.Column<string>(maxLength: 20, nullable: false),
                    Snaga = table.Column<string>(maxLength: 20, nullable: false),
                    Kilometraza = table.Column<string>(unicode: false, maxLength: 6, nullable: false),
                    Pogon = table.Column<string>(maxLength: 20, nullable: false),
                    Menjac = table.Column<string>(maxLength: 20, nullable: false),
                    BrojBrzina = table.Column<string>(unicode: false, maxLength: 1, nullable: true),
                    Cena = table.Column<int>(nullable: false),
                    Slika = table.Column<byte[]>(nullable: true),
                    SlikaTip = table.Column<string>(maxLength: 20, nullable: true),
                    Opis = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vozilo", x => x.VoziloId);
                    table.ForeignKey(
                        name: "FK__Vozilo__MarkaId__5070F446",
                        column: x => x.MarkaId,
                        principalTable: "Marka",
                        principalColumn: "MarkaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Vozilo__ModelId__5165187F",
                        column: x => x.ModelId,
                        principalTable: "Modeli",
                        principalColumn: "ModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Vozilo__TipVozil__52593CB8",
                        column: x => x.TipVozilaId,
                        principalTable: "TipVozila",
                        principalColumn: "TipVozilaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Komentar",
                columns: table => new
                {
                    KomentarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VoziloId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<string>(maxLength: 450, nullable: false),
                    Korisnik = table.Column<string>(maxLength: 20, nullable: false),
                    Opis = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentar", x => x.KomentarId);
                    table.ForeignKey(
                        name: "FK__Komentar__Vozilo__5535A963",
                        column: x => x.VoziloId,
                        principalTable: "Vozilo",
                        principalColumn: "VoziloId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_VoziloId",
                table: "Komentar",
                column: "VoziloId");

            migrationBuilder.CreateIndex(
                name: "IX_Modeli_MarkaId",
                table: "Modeli",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_MarkaId",
                table: "Vozilo",
                column: "MarkaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_ModelId",
                table: "Vozilo",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vozilo_TipVozilaId",
                table: "Vozilo",
                column: "TipVozilaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Komentar");

            migrationBuilder.DropTable(
                name: "Vozilo");

            migrationBuilder.DropTable(
                name: "Modeli");

            migrationBuilder.DropTable(
                name: "TipVozila");

            migrationBuilder.DropTable(
                name: "Marka");
        }
    }
}
