
namespace ApiCatalogoTestesUnitarios.RepositoryTests
{
    public class RepositoryTestesUnitarios
    {
        [Fact]
        public void Add_Entity_QuandoChamarAdd_DeveSalvarEntidadeNova()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Produto>>();
            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockContext.Setup(m => m.Set<Produto>()).Returns(mockSet.Object);

            var repository = new Repository<Produto>(mockContext.Object);
            var produto = new Produto();

            // Act
            repository.Add(produto);

            // Assert
            mockSet.Verify(m => m.Add(It.IsAny<Produto>()), Times.Once());
        }

        [Fact]
        public void GetAll_DeveRetornarListaDeProdutos()
        {
            // Arrange
            var data = new List<Produto>
            {
                new Produto { Nome = "Produto 1" },
                new Produto { Nome = "Produto 2" },
                new Produto { Nome = "Produto 3" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Produto>>();
            mockSet.As<IQueryable<Produto>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Produto>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Produto>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Produto>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockContext.Setup(c => c.Set<Produto>()).Returns(mockSet.Object);

            var repository = new Repository<Produto>(mockContext.Object);

            // Act
            var result = repository.GetAll();

            // Assert
            Assert.Equal(3, result.Count());
            Assert.Contains(result, p => p.Nome == "Produto 1");
            Assert.Contains(result, p => p.Nome == "Produto 2");
            Assert.Contains(result, p => p.Nome == "Produto 3");
        }

        [Fact]
        public async Task GetById_DeveRetornarEntidade_QuandoEntidadeExiste()
        {
            // Arrange
            var data = new List<Produto>
            {
                new Produto { ProdutoId = 1, Nome = "Produto 1" , Preco = 10, Estoque =100},
                new Produto { ProdutoId = 2, Nome = "Produto 2" , Preco = 22, Estoque =200},
                new Produto { ProdutoId = 3, Nome = "Produto 3" , Preco = 33, Estoque =300}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Produto>>();
            mockSet.As<IAsyncEnumerable<Produto>>().Setup(m => m.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<Produto>(data.GetEnumerator()));
            mockSet.As<IQueryable<Produto>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Produto>(data.Provider));
            mockSet.As<IQueryable<Produto>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Produto>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Produto>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockContext.Setup(c => c.Set<Produto>()).Returns(mockSet.Object);

            var repository = new Repository<Produto>(mockContext.Object);

            // Act
            var result = await repository.GetById(p => p.ProdutoId == 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Produto 2", result.Nome);
            Assert.Equal(22, result.Preco);
            Assert.Equal(200, result.Estoque);
            Assert.Equal(2, result.ProdutoId);
        }       

        [Fact]
        public async Task GetById_DeveRetornarNull_QuandoEntidadeNaoExiste()
        {
            // Arrange
            var data = new List<Produto>
            {
                new Produto { ProdutoId = 1, Nome = "Produto 1" , Preco = 10, Estoque =100},
                new Produto { ProdutoId = 2, Nome = "Produto 2" , Preco = 22, Estoque =200},
                new Produto { ProdutoId = 3, Nome = "Produto 3" , Preco = 33, Estoque =300}
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Produto>>();
            mockSet.As<IAsyncEnumerable<Produto>>().Setup(m => m.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<Produto>(data.GetEnumerator()));
            mockSet.As<IQueryable<Produto>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Produto>(data.Provider));
            mockSet.As<IQueryable<Produto>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Produto>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Produto>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockContext.Setup(c => c.Set<Produto>()).Returns(mockSet.Object);

            var repository = new Repository<Produto>(mockContext.Object);

            // Act
            var result = await repository.GetById(p => p.ProdutoId == 4);

            // Assert            
            Assert.True(result == null);
        }

        [Fact]
        public async Task Update_Entity_QuandoAtualizacaoProdutoComSucesso()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new AppDbContext(options))
            {
                var repository = new Repository<Produto>(context);
                var produto = new Produto { ProdutoId = 1, Nome = "Produto Original", Descricao = "Um produto qualquer" };
                context.Set<Produto>().Add(produto);
                await context.SaveChangesAsync();

                // Act
                produto.Nome = "Produto Atualizado";
                repository.Update(produto);
                await context.SaveChangesAsync();

                // Assert
                var updatedProduto = await context.Set<Produto>().FindAsync(produto.ProdutoId);
                Assert.Equal("Produto Atualizado", updatedProduto.Nome);
            }
        }

        [Fact]
        public void Delete_Entity_QuandoEntidadeForDeletadaComSucesso()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Produto>>();
            var mockContext = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockContext.Setup(m => m.Set<Produto>()).Returns(mockSet.Object);
            
            var repository = new Repository<Produto>(mockContext.Object);
            var produto = new Produto { ProdutoId = 3, Nome = "Produto 3", Preco = 33, Estoque = 300 }; 

            // Act
            repository.Delete(produto);

            // Assert
            mockSet.Verify(m => m.Remove(It.IsAny<Produto>()), Times.Once());
            mockSet.Verify(m => m.Add(It.IsAny<Produto>()), Times.Never());
            mockSet.Verify(m => m.Update(It.IsAny<Produto>()), Times.Never());
        }

    }
    public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }

    public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return ValueTask.FromResult(_enumerator.MoveNext());
        }
    }

    public class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        public TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                .GetMethod(
                    name: nameof(IQueryProvider.Execute),
                    genericParameterCount: 1,
                    types: new[] { typeof(Expression) })
                .MakeGenericMethod(expectedResultType)
                .Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                ?.MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult });
        }
    }

}
