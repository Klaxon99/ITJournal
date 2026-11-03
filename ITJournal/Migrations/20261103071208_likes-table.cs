using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITJournal.Migrations
{
    /// <inheritdoc />
    public partial class likestable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_Articles_ArticleId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_Users_AuthorId",
                table: "Like");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.RenameTable(
                name: "Like",
                newName: "Likes");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Likes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Likes",
                newName: "LikeId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_AuthorId",
                table: "Likes",
                newName: "IX_Likes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Like_ArticleId",
                table: "Likes",
                newName: "IX_Likes_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Likes",
                table: "Likes",
                column: "LikeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Articles_ArticleId",
                table: "Likes");

            migrationBuilder.DropForeignKey(
                name: "FK_Likes_Users_UserId",
                table: "Likes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Likes",
                table: "Likes");

            migrationBuilder.RenameTable(
                name: "Likes",
                newName: "Like");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Like",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "LikeId",
                table: "Like",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_UserId",
                table: "Like",
                newName: "IX_Like_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Likes_ArticleId",
                table: "Like",
                newName: "IX_Like_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Articles_ArticleId",
                table: "Like",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Users_AuthorId",
                table: "Like",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
