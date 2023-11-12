using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScmssApiServer.Migrations
{
    /// <inheritdoc />
    public partial class RenameUpdateProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdatedTime",
                table: "PurchaseRequisitions",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "PurchaseRequisitions",
                newName: "CreateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedTime",
                table: "PurchaseOrders",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "PurchaseOrders",
                newName: "CreateTime");

            migrationBuilder.RenameColumn(
                name: "UpdatedTime",
                table: "AspNetUsers",
                newName: "UpdateTime");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "AspNetUsers",
                newName: "CreateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "PurchaseRequisitions",
                newName: "UpdatedTime");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "PurchaseRequisitions",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "PurchaseOrders",
                newName: "UpdatedTime");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "PurchaseOrders",
                newName: "CreatedTime");

            migrationBuilder.RenameColumn(
                name: "UpdateTime",
                table: "AspNetUsers",
                newName: "UpdatedTime");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "AspNetUsers",
                newName: "CreatedTime");
        }
    }
}
