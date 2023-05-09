using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendaceSys.Data.Migrations
{
    public partial class AddedPictureUrlForEmp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "TblEmployee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "TblEmployee");
        }
    }
}
