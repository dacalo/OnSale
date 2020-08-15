using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnSale.Web.Migrations
{
    public partial class ChangeImageProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ProductImages");

            migrationBuilder.AddColumn<string>(
                name: "UrlImage",
                table: "ProductImages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImage",
                table: "ProductImages");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "ProductImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
