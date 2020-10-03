using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Cubipool.Repository.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Constants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RelatedTableId = table.Column<int>(nullable: false),
                    RelatedTableName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    IsInternal = table.Column<bool>(nullable: false),
                    IsEditable = table.Column<bool>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Dependency = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PointsNeeded = table.Column<int>(nullable: false),
                    MaxInstances = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentCode = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    MaxHoursPerDay = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cubicles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TotalSeats = table.Column<int>(nullable: false),
                    Pavilion = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CampusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cubicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cubicles_Constants_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Constants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointsRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Points = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointsRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ConstantId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Constants_ConstantId",
                        column: x => x.ConstantId,
                        principalTable: "Constants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPrizes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Quantity = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    PrizeId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPrizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPrizes_Prizes_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPrizes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    HostLeaveTime = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CubicleId = table.Column<int>(nullable: false),
                    ReservationStateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Cubicles_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "Cubicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Constants_ReservationStateId",
                        column: x => x.ReservationStateId,
                        principalTable: "Constants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Points = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ResourceTypeId = table.Column<int>(nullable: false),
                    CubicleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resources_Cubicles_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "Cubicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Resources_Constants_ResourceTypeId",
                        column: x => x.ResourceTypeId,
                        principalTable: "Constants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    SharedSeats = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ReservationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Publications_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UserRoleId = table.Column<int>(nullable: false),
                    ReservationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReservations_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReservations_Constants_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "Constants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedSpaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    IsOccupied = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    PublicationId = table.Column<int>(nullable: false),
                    ResourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedSpaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedSpaces_Publications_PublicationId",
                        column: x => x.PublicationId,
                        principalTable: "Publications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedSpaces_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SharedSpaceRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    SharedSpaceId = table.Column<int>(nullable: false),
                    RequestId = table.Column<int>(nullable: false)
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
                name: "IX_Cubicles_CampusId",
                table: "Cubicles",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsRecords_UserId",
                table: "PointsRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Publications_ReservationId",
                table: "Publications",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ConstantId",
                table: "Requests",
                column: "ConstantId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserId",
                table: "Requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CubicleId",
                table: "Reservations",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationStateId",
                table: "Reservations",
                column: "ReservationStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_CubicleId",
                table: "Resources",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_ResourceTypeId",
                table: "Resources",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedSpaceRequests_RequestId",
                table: "SharedSpaceRequests",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedSpaceRequests_SharedSpaceId",
                table: "SharedSpaceRequests",
                column: "SharedSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedSpaces_PublicationId",
                table: "SharedSpaces",
                column: "PublicationId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedSpaces_ResourceId",
                table: "SharedSpaces",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrizes_PrizeId",
                table: "UserPrizes",
                column: "PrizeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPrizes_UserId",
                table: "UserPrizes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_ReservationId",
                table: "UserReservations",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_UserId",
                table: "UserReservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_UserRoleId",
                table: "UserReservations",
                column: "UserRoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointsRecords");

            migrationBuilder.DropTable(
                name: "SharedSpaceRequests");

            migrationBuilder.DropTable(
                name: "UserPrizes");

            migrationBuilder.DropTable(
                name: "UserReservations");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "SharedSpaces");

            migrationBuilder.DropTable(
                name: "Prizes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Publications");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Cubicles");

            migrationBuilder.DropTable(
                name: "Constants");
        }
    }
}
