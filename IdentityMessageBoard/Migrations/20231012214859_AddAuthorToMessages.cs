using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityMessageBoard.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorToMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "author_id",
                table: "messages",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_messages_author_id",
                table: "messages",
                column: "author_id");

            migrationBuilder.AddForeignKey(
                name: "fk_messages_users_author_id",
                table: "messages",
                column: "author_id",
                principalTable: "AspNetUsers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_users_author_id",
                table: "messages");

            migrationBuilder.DropIndex(
                name: "ix_messages_author_id",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "author_id",
                table: "messages");
        }
    }
}
