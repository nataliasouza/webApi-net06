
namespace ApiCatalogoTestesUnitarios
{
    public class DbMockInitializerIntegrationTests
    {
        public DbMockInitializerIntegrationTests()
        {}

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add(
                new Categoria { 
                    CategoriaId = 1,
                    Nome = "Bebidas",
                    ImagemUrl = "bebidas.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 2,
                    Nome = "Pizzas",
                    ImagemUrl = "pizzas.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 3,
                    Nome = "Salgados",
                    ImagemUrl = "salgados.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 4,
                    Nome = "Bolos",
                    ImagemUrl = "bolos.png"
                });

            context.Categorias.Add(
                new Categoria
                {
                    CategoriaId = 5,
                    Nome = "Chocolates",
                    ImagemUrl = "chocolates.png"
                });

            context.SaveChanges();
        }
    }
}
