using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnSale.Web.Migrations
{
    public partial class ChangeImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Departments_DepartmentId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Countries_CountryId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Name",
                table: "Departments");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name_CountryId",
                table: "Departments",
                columns: new[] { "Name", "CountryId" },
                unique: true,
                filter: "[CountryId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Departments_DepartmentId",
                table: "Cities",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Countries_CountryId",
                table: "Departments",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Departments_DepartmentId",
                table: "Cities");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Countries_CountryId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Name_CountryId",
                table: "Departments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ImageId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Departments_DepartmentId",
                table: "Cities",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Countries_CountryId",
                table: "Departments",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
