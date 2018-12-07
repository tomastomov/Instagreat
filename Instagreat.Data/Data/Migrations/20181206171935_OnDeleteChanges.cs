using Microsoft.EntityFrameworkCore.Migrations;

namespace Instagreat.Data.Migrations
{
    public partial class OnDeleteChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_AspNetUsers_UserId",
                table: "CommentReplies");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_AspNetUsers_UserId",
                table: "CommentReplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentReplies_AspNetUsers_UserId",
                table: "CommentReplies");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentReplies_AspNetUsers_UserId",
                table: "CommentReplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
