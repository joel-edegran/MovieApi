using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameReviewnameToReviewerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reviewname",
                table: "Reviews",
                newName: "ReviewerName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewerName",
                table: "Reviews",
                newName: "Reviewname");
        }
    }
}
