using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CetinBurger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMailSentStatusToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MailSentStatus",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailSentStatus",
                table: "Orders");
        }
    }
}
