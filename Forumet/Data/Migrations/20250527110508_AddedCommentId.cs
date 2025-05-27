using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forumet.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCommentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CommentId",
                table: "Reports",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Comments_CommentId",
                table: "Reports",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Comments_CommentId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CommentId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Reports");
        }
    }
}
