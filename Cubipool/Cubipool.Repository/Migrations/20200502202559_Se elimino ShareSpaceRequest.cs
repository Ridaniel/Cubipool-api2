using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cubipool.Repository.Migrations
{
    public partial class SeeliminoShareSpaceRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedSpaceRequests");

            migrationBuilder.AddColumn<int>(
                name: "SharedSpaceId",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SharedSpaceId",
                table: "Requests",
                column: "SharedSpaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_SharedSpaces_SharedSpaceId",
                table: "Requests",
                column: "SharedSpaceId",
                principalTable: "SharedSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_SharedSpaces_SharedSpaceId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SharedSpaceId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SharedSpaceId",
                table: "Requests");

            migrationBuilder.CreateTable(
                name: "SharedSpaceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: false),
                    SharedSpaceId = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedSpaceRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedSpaceRequests_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedSpaceRequests_SharedSpaces_SharedSpaceId",
                        column: x => x.SharedSpaceId,
                        principalTable: "SharedSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedSpaceRequests_RequestId",
                table: "SharedSpaceRequests",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedSpaceRequests_SharedSpaceId",
                table: "SharedSpaceRequests",
                column: "SharedSpaceId");
        }
    }
}
