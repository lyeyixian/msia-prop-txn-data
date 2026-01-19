using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MsiaPropertyTransaction.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "property_transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    property_type = table.Column<string>(type: "text", nullable: false),
                    district = table.Column<string>(type: "text", nullable: false),
                    mukim = table.Column<string>(type: "text", nullable: false),
                    scheme_name_area = table.Column<string>(type: "text", nullable: false),
                    road_name = table.Column<string>(type: "text", nullable: true),
                    tenure = table.Column<string>(type: "text", nullable: false),
                    land_parcel_area = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    unit = table.Column<string>(type: "text", nullable: true),
                    main_floor_area = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    unit_level = table.Column<string>(type: "text", nullable: true),
                    property_type_strata = table.Column<string>(type: "text", nullable: false),
                    sector = table.Column<string>(type: "text", nullable: false),
                    state = table.Column<string>(type: "text", nullable: false),
                    transaction_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_property_transactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "property_transactions");
        }
    }
}
