using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcProekt.Migrations
{
    /// <inheritdoc />
    public partial class MyBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_MyBooks_MyBooksId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_MyBooksId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "MyBooksId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "MyBooksId",
                table: "UserBooks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MyBooksId",
                table: "Review",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "MyBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "AverageRating",
                table: "MyBooks",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MyBooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DownloadUrl",
                table: "MyBooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontPage",
                table: "MyBooks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumPages",
                table: "MyBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "MyBooks",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MyBooks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearPublished",
                table: "MyBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MyBooksId",
                table: "BookGenre",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserBooks_MyBooksId",
                table: "UserBooks",
                column: "MyBooksId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_MyBooksId",
                table: "Review",
                column: "MyBooksId");

            migrationBuilder.CreateIndex(
                name: "IX_MyBooks_AuthorId",
                table: "MyBooks",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_MyBooksId",
                table: "BookGenre",
                column: "MyBooksId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookGenre_MyBooks_MyBooksId",
                table: "BookGenre",
                column: "MyBooksId",
                principalTable: "MyBooks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MyBooks_Author_AuthorId",
                table: "MyBooks",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_MyBooks_MyBooksId",
                table: "Review",
                column: "MyBooksId",
                principalTable: "MyBooks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBooks_MyBooks_MyBooksId",
                table: "UserBooks",
                column: "MyBooksId",
                principalTable: "MyBooks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookGenre_MyBooks_MyBooksId",
                table: "BookGenre");

            migrationBuilder.DropForeignKey(
                name: "FK_MyBooks_Author_AuthorId",
                table: "MyBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_MyBooks_MyBooksId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBooks_MyBooks_MyBooksId",
                table: "UserBooks");

            migrationBuilder.DropIndex(
                name: "IX_UserBooks_MyBooksId",
                table: "UserBooks");

            migrationBuilder.DropIndex(
                name: "IX_Review_MyBooksId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_MyBooks_AuthorId",
                table: "MyBooks");

            migrationBuilder.DropIndex(
                name: "IX_BookGenre_MyBooksId",
                table: "BookGenre");

            migrationBuilder.DropColumn(
                name: "MyBooksId",
                table: "UserBooks");

            migrationBuilder.DropColumn(
                name: "MyBooksId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "DownloadUrl",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "FrontPage",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "NumPages",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "YearPublished",
                table: "MyBooks");

            migrationBuilder.DropColumn(
                name: "MyBooksId",
                table: "BookGenre");

            migrationBuilder.AddColumn<int>(
                name: "MyBooksId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_MyBooksId",
                table: "Books",
                column: "MyBooksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_MyBooks_MyBooksId",
                table: "Books",
                column: "MyBooksId",
                principalTable: "MyBooks",
                principalColumn: "Id");
        }
    }
}
