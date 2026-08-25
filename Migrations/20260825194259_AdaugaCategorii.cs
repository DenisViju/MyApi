using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Migrations
{
    /// <inheritdoc />
    public partial class AdaugaCategorii : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Produse",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategorieId",
                table: "Produse",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categorii",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nume = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorii", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produse_CategorieId",
                table: "Produse",
                column: "CategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produse_Categorii_CategorieId",
                table: "Produse",
                column: "CategorieId",
                principalTable: "Categorii",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produse_Categorii_CategorieId",
                table: "Produse");

            migrationBuilder.DropTable(
                name: "Categorii");

            migrationBuilder.DropIndex(
                name: "IX_Produse_CategorieId",
                table: "Produse");

            migrationBuilder.DropColumn(
                name: "CategorieId",
                table: "Produse");

            migrationBuilder.AlterColumn<string>(
                name: "Nume",
                table: "Produse",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);
        }
    }
}
