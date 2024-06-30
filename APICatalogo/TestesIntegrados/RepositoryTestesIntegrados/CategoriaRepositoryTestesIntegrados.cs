
namespace TestesIntegrados.RepositoryTestesIntegrados
{
    public class CategoriaRepositoryTestesIntegrados
    {
        private readonly DbMockInitializerIntegrationTests _dbInitializer;
        private readonly AppDbContext _context;
        public CategoriaRepositoryTestesIntegrados()
        {
            var dbName = $"DbInMemoryTest_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            _context = new AppDbContext(options);

            _dbInitializer = new DbMockInitializerIntegrationTests();
            _dbInitializer.Seed(_context);
        }

        [Fact]
        public async Task GetTodasCategoriasRepository_DeveRetornarTodasCategoriasPaginadas()
        {
            // Arrange
            var repository = new CategoriaRepository(_context);
            var categoriasParameters = new CategoriasParameters { PageNumber = 1, PageSize = 5 };

            // Act
            var result = await repository.GetTodasCategoriasRepository(categoriasParameters);

            // Assert                      
            ValidaCategoria(result, 1, "Bebidas");
            Assert.Contains(result, c => c.ImagemUrl == "bebidas.png");
            ValidaCategoria(result, 2, "Pizzas");
            Assert.Contains(result, c => c.ImagemUrl == "pizzas.png");
            ValidaCategoria(result, 3, "Salgados");
            Assert.Contains(result, c => c.ImagemUrl == "salgados.png");
            ValidaCategoria(result, 4, "Bolos");
            Assert.Contains(result, c => c.ImagemUrl == "bolos.png");
            ValidaCategoria(result, 5, "Chocolates");
            Assert.Contains(result, c => c.ImagemUrl == "chocolates.png");

            // Paginação
            categoriasParameters.PageNumber = 1;
            categoriasParameters.PageSize = 5;
            result = await repository.GetTodasCategoriasRepository(categoriasParameters);
            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
            Assert.Equal(5, result.TotalCount);
            Assert.Equal(5, result.PageSize);
            Assert.Equal(1, result.CurrentPage);
            Assert.Equal(1, result.TotalPages);
            Assert.False(result.HasPrevious);         

        }

        private void ValidaCategoria(IEnumerable<Categoria> categorias, int expectedId, string expectedNome)
        {
            var categoria = categorias.FirstOrDefault(c => c.Nome == expectedNome);
            Assert.NotNull(categoria); // Verifica se a categoria foi encontrada
            Assert.Equal(expectedId, categoria.CategoriaId); // Verifica se o ID da categoria corresponde ao esperado
        }

        [Fact]
        public async Task GetCategoriasComProdutosRepository_RetornaTodasCategoriasComProdutos()
        {
            // Arrange
            var repository = new CategoriaRepository(_context);

            // Act
            var categorias = await repository.GetCategoriasComProdudosRepository();

            // Assert
            Assert.NotNull(categorias);
            Assert.NotEmpty(categorias);            
        }
    }
}