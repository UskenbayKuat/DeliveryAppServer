using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    public partial class InitialModel4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnReview",
                table: "ClientPackages");

            migrationBuilder.AddColumn<int>(
                name: "OnDriverReviewId",
                table: "ClientPackages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OnDriverReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OnReview = table.Column<bool>(type: "boolean", nullable: false),
                    RouteTripId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnDriverReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnDriverReviews_RouteTrips_RouteTripId",
                        column: x => x.RouteTripId,
                        principalTable: "RouteTrips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientPackages_OnDriverReviewId",
                table: "ClientPackages",
                column: "OnDriverReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_OnDriverReviews_RouteTripId",
                table: "OnDriverReviews",
                column: "RouteTripId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPackages_OnDriverReviews_OnDriverReviewId",
                table: "ClientPackages",
                column: "OnDriverReviewId",
                principalTable: "OnDriverReviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientPackages_OnDriverReviews_OnDriverReviewId",
                table: "ClientPackages");

            migrationBuilder.DropTable(
                name: "OnDriverReviews");

            migrationBuilder.DropIndex(
                name: "IX_ClientPackages_OnDriverReviewId",
                table: "ClientPackages");

            migrationBuilder.DropColumn(
                name: "OnDriverReviewId",
                table: "ClientPackages");

            migrationBuilder.AddColumn<bool>(
                name: "OnReview",
                table: "ClientPackages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
