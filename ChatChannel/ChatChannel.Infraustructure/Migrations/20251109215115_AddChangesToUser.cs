using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatChannel.Infraustructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChangesToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HaveUnreadMessage",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HaveUnreadMessage",
                table: "Users");
        }
    }
}
