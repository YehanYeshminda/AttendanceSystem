using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAttendaceSys.Data.Migrations
{
    public partial class AddedSomeValidations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Attendances",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RegNoEmp",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Attendances");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "TblEmployee");

            migrationBuilder.RenameTable(
                name: "Attendances",
                newName: "TblAttendace");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "TblEmployee",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "TblAttendace",
                newName: "InTime");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "TblEmployee",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "TblEmployee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfJoin",
                table: "TblEmployee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "TblEmployee",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnrollNo",
                table: "TblEmployee",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegNo",
                table: "TblEmployee",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RegNo",
                table: "TblAttendace",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AddBy",
                table: "TblAttendace",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "EnrollNo",
                table: "TblAttendace",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MachineFinger",
                table: "TblAttendace",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblEmployee",
                table: "TblEmployee",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TblAttendace",
                table: "TblAttendace",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TblEmployee_RegNo",
                table: "TblEmployee",
                column: "RegNo",
                unique: true,
                filter: "[RegNo] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TblEmployee",
                table: "TblEmployee");

            migrationBuilder.DropIndex(
                name: "IX_TblEmployee_RegNo",
                table: "TblEmployee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TblAttendace",
                table: "TblAttendace");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "TblEmployee");

            migrationBuilder.DropColumn(
                name: "DateOfJoin",
                table: "TblEmployee");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "TblEmployee");

            migrationBuilder.DropColumn(
                name: "EnrollNo",
                table: "TblEmployee");

            migrationBuilder.DropColumn(
                name: "RegNo",
                table: "TblEmployee");

            migrationBuilder.DropColumn(
                name: "EnrollNo",
                table: "TblAttendace");

            migrationBuilder.DropColumn(
                name: "MachineFinger",
                table: "TblAttendace");

            migrationBuilder.RenameTable(
                name: "TblEmployee",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "TblAttendace",
                newName: "Attendances");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Employees",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "InTime",
                table: "Attendances",
                newName: "Time");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Mobile",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegNoEmp",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegNo",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AddBy",
                table: "Attendances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attendances",
                table: "Attendances",
                column: "Id");
        }
    }
}
