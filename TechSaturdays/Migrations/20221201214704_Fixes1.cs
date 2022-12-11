using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechSaturdays.Migrations
{
    public partial class Fixes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGroup_Groups_GroupsLoctoredGroupId",
                table: "ApplicationUserGroup");

            migrationBuilder.DropColumn(
                name: "BannedUntil",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Lastname",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Firstname",
                table: "AspNetUsers",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "GroupsLoctoredGroupId",
                table: "ApplicationUserGroup",
                newName: "GroupsLectoredGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGroup_Groups_GroupsLectoredGroupId",
                table: "ApplicationUserGroup",
                column: "GroupsLectoredGroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGroup_Groups_GroupsLectoredGroupId",
                table: "ApplicationUserGroup");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "Lastname");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "Firstname");

            migrationBuilder.RenameColumn(
                name: "GroupsLectoredGroupId",
                table: "ApplicationUserGroup",
                newName: "GroupsLoctoredGroupId");

            migrationBuilder.AddColumn<DateTime>(
                name: "BannedUntil",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGroup_Groups_GroupsLoctoredGroupId",
                table: "ApplicationUserGroup",
                column: "GroupsLoctoredGroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
