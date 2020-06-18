using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VT.Data.Migrations
{
    public partial class ModifyDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_Users_DeletedByUserId",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_SubTypes_Users_DeletedByUserId",
                table: "SubTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Torrents_Users_DeletedByUserId",
                table: "Torrents");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_DeletedByUserId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToTorrents_Users_DeletedByUserId",
                table: "UserToTorrents");

            migrationBuilder.DropIndex(
                name: "IX_UserToTorrents_DeletedByUserId",
                table: "UserToTorrents");

            migrationBuilder.DropIndex(
                name: "IX_Users_DeletedByUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Torrents_DeletedByUserId",
                table: "Torrents");

            migrationBuilder.DropIndex(
                name: "IX_SubTypes_DeletedByUserId",
                table: "SubTypes");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_DeletedByUserId",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "UserToTorrents");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Torrents");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "SubTypes");

            migrationBuilder.DropColumn(
                name: "DeletedByUserId",
                table: "Catalogs");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "UserToTorrents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Torrents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "SubTypes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Catalogs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "UserToTorrents");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Torrents");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "SubTypes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Catalogs");

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "UserToTorrents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Torrents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "SubTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedByUserId",
                table: "Catalogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserToTorrents_DeletedByUserId",
                table: "UserToTorrents",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedByUserId",
                table: "Users",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Torrents_DeletedByUserId",
                table: "Torrents",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTypes_DeletedByUserId",
                table: "SubTypes",
                column: "DeletedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_DeletedByUserId",
                table: "Catalogs",
                column: "DeletedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_Users_DeletedByUserId",
                table: "Catalogs",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubTypes_Users_DeletedByUserId",
                table: "SubTypes",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Torrents_Users_DeletedByUserId",
                table: "Torrents",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_DeletedByUserId",
                table: "Users",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToTorrents_Users_DeletedByUserId",
                table: "UserToTorrents",
                column: "DeletedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
