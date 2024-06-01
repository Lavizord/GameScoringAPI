using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameScoringAPI.Migrations
{
    /// <inheritdoc />
    public partial class GameMatchCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matche_Games_GameId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchDataPoints_Matche_MatchId",
                table: "MatchDataPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchDataPoints",
                table: "MatchDataPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matche",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "MatchDataPoints",
                newName: "MatchDataPoints");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Games");

            migrationBuilder.RenameIndex(
                name: "IX_MatchDataPoints_MatchId",
                table: "MatchDataPoints",
                newName: "IX_MatchDataPoints_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_GameId",
                table: "Matches",
                newName: "IX_Matches_GameId");

            migrationBuilder.AddColumn<int>(
                name: "MatchesCount",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchDataPoints",
                table: "MatchDataPoints",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchDataPoints_Matches_MatchId",
                table: "MatchDataPoints",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Games_GameId",
                table: "Matches",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchDataPoints_Matches_MatchId",
                table: "MatchDataPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Games_GameId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchDataPoints",
                table: "MatchDataPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "MatchesCount",
                table: "Games");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "MatchDataPoints",
                newName: "MatchDataPoints");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Games");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_GameId",
                table: "Matches",
                newName: "IX_Matches_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchDataPoints_MatchId",
                table: "MatchDataPoints",
                newName: "IX_MatchDataPoints_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matche",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchDataPoints",
                table: "MatchDataPoints",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matche_Games_GameId",
                table: "Matches",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchDataPoints_Matche_MatchId",
                table: "MatchDataPoints",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
