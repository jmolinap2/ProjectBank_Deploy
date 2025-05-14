using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectBlack.Migrations
{
    /// <inheritdoc />
    public partial class Added_CreditRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontoSolicitado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlazoMeses = table.Column<int>(type: "int", nullable: false),
                    IngresoMensual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AntiguedadLaboral = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditRequests");
        }
    }
}
