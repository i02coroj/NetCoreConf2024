using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetConf2024Search.Seeder.Migrations
{
    /// <inheritdoc />
    public partial class AddQueryBooksView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				CREATE VIEW [dbo].[GetBooksIndexData] AS
				SELECT 
					b.Id AS Id,
					b.Title AS Title,
					b.Reference AS reference,
					b.Summary AS Summary,
					b.Gendre AS Gendre,
					b.PublishedOn AS PublishedOn,
					b.InStock AS InStock,
					b.NumberOfPages AS NumberOfPages,
					b.Price AS Price,
					(SELECT value AS LanguageName FROM STRING_SPLIT(b.Languages, ',') FOR JSON PATH) AS Languages,
					b.LastModifiedDt,
					a.FirstName AS AuthorFirstName,
					a.LastName AS AuthorLastName,
					a.Bio AS AuthorBio,
					CommentJson.[JSON] AS Comments
				FROM dbo.Books b
				INNER JOIN dbo.Authors a ON a.Id = b.AuthorId
				OUTER APPLY (SELECT(SELECT c.Text [Text], c.Author [Author], c.PublishedOn [PublishedOn]
				FROM dbo.Comments AS c
				WHERE c.BookId = b.Id FOR JSON PATH) [JSON]) CommentJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[GetBooksIndexData]");
        }
    }
}
