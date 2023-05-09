using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendaceSys.Data.Migrations
{
    public partial class AddNewEmployeeImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManualAttendance",
                table: "TblAttendace",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "TblAttendace",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ManualAttendance",
                table: "TblAttendace");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "TblAttendace");
        }
    }
}
