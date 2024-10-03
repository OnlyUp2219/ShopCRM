using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopCRM.Migrations
{
   /// <inheritdoc />
   public partial class Initial : Migration
   {
      /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.CreateTable(
             name: "Clients",
             columns: table => new
             {
                Id = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Company = table.Column<string>(type: "nvarchar(max)", nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Clients", x => x.Id);
             });

         migrationBuilder.CreateTable(
             name: "Orders",
             columns: table => new
             {
                Id = table.Column<int>(type: "int", nullable: false)
                     .Annotation("SqlServer:Identity", "1, 1"),
                ClientId = table.Column<int>(type: "int", nullable: false),
                OrderDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
             },
             constraints: table =>
             {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.ForeignKey(
                       name: "FK_Orders_Clients_ClientId",
                       column: x => x.ClientId,
                       principalTable: "Clients",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
             });

         migrationBuilder.CreateIndex(
             name: "IX_Orders_ClientId",
             table: "Orders",
             column: "ClientId");
      }

      /// <inheritdoc />
      protected override void Down(MigrationBuilder migrationBuilder)
      {
         migrationBuilder.DropTable(
             name: "Orders");

         migrationBuilder.DropTable(
             name: "Clients");
      }
   }
}
