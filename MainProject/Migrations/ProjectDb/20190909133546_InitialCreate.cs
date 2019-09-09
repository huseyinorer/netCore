using Microsoft.EntityFrameworkCore.Migrations;

namespace MainProject.Migrations.ProjectDb
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    WebSiteName = table.Column<string>(nullable: false),
                    Credentials_Email = table.Column<string>(nullable: true),
                    Credentials_Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.WebSiteName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
