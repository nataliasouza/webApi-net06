using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    public partial class PopulaCategorias : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Bebidas','bebidas.jpg')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Lanches', 'lanches.png')");
            mb.Sql("Insert into Categorias(Nome, ImagemUrl) Values('Pizzas', 'pizza.jpg')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
        }
    }
}
