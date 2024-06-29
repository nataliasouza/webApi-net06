
namespace ApiCatalogoTestesUnitarios.ProdutosControllerTests
{
    public class ProdutosControllerTestesUnitarios
    {
        private readonly Mock<IUnitOfWork> _mockUof;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<ProdutosController>> _mockLogger;
        private readonly ProdutosController _controller;

        public ProdutosControllerTestesUnitarios()
        {
            _mockUof = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<ProdutosController>>();
            _controller = new ProdutosController(_mockUof.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetProdutosPrecos_ReturnsOKResult_DeveRetornarProdutosOrdenadosPorPreco()
        {
            // Arrange
            var fakeProdutos = new List<Produto> 
            { 
                new Produto
                        {
                            ProdutoId = 1,
                            Nome = "Coca-Cola",
                            Descricao = "Refrigerante de cola",
                            Preco = 5,
                            ImagemUrl = "coca.png"
                        },
                        new Produto
                        {
                            ProdutoId = 2,
                            Nome = "Água",
                            Descricao = "Água Gaseificada",
                            Preco = 4,
                            ImagemUrl = "agua.png"
                        },
                        new Produto
                        {
                            ProdutoId = 3,
                            Nome = "Bombom",
                            Descricao = "Bombom",
                            Preco = 2,
                            ImagemUrl = "bombom.png"
                        }
            };
            
            var fakeProdutosDTO = fakeProdutos.Select(p => new ProdutoDTO
            {
                ProdutoId = p.ProdutoId,
                Nome = p.Nome,
                Preco = p.Preco
            }).OrderBy(p => p.Preco).ToList();

            _mockUof.Setup(uof => uof.ProdutoRepository.GetProdutosPorPreco())
                .ReturnsAsync(fakeProdutos.OrderBy(p => p.Preco).ToList());

            _mockMapper.Setup(m => m.Map<List<ProdutoDTO>>(It.IsAny<List<Produto>>()))
                .Returns(fakeProdutosDTO);

            // Act
            var result = await _controller.GetProdutosPrecos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProdutoDTO>>(okResult.Value);
            Assert.Equal(fakeProdutosDTO.Count, returnValue.Count);
            Assert.Equal("Bombom", fakeProdutosDTO[0].Nome);
            Assert.Equal(2, fakeProdutosDTO[0].Preco);
            Assert.True(fakeProdutosDTO[0].Preco < fakeProdutosDTO[1].Preco);
            Assert.Equal("Água", fakeProdutosDTO[1].Nome);
            Assert.Equal(4, fakeProdutosDTO[1].Preco);
            Assert.True(fakeProdutosDTO[1].Preco < fakeProdutosDTO[2].Preco);
            Assert.Equal("Coca-Cola", fakeProdutosDTO[2].Nome);
            Assert.Equal(5, fakeProdutosDTO[2].Preco);            
            Assert.Equal(200, okResult.StatusCode);
        }
        [Fact]
        public async Task GetProdutosPrecos_ThrowsException_DeveRetornarMensagemDeExcecao()
        {
            // Arrange
            var exceptionMessage = "Ocorreu um erro ao buscar a lista de produto ordenada com o menor preço.";
            _mockUof.Setup(uof => uof.ProdutoRepository.GetProdutosPorPreco())
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetProdutosPrecos());
            Assert.Equal(exceptionMessage, exception.Message);
        }

        [Fact]
        public async Task GetTodosProdutos_ReturnsPaginatedList_DeveRetornarProdutosPaginados()
        {
            // Arrange
            
            var produtosParameters = new ProdutosParameters();
            
            var produtos = new PagedList<Produto>(new List<Produto>
                {
                    new Produto { ProdutoId = 1, Nome = "Produto C", Preco = 10.0M },
                    new Produto { ProdutoId = 2, Nome = "Produto A", Preco = 5.0M }
                }, 2, 1, 10);
                
            var produtosDTO = new List<ProdutoDTO>
                {
                    new ProdutoDTO { ProdutoId = 1, Nome = "Produto C", Preco = 10.0M },
                    new ProdutoDTO {ProdutoId = 2, Nome = "Produto A", Preco = 5.0M}
                };

            _mockUof.Setup(uof => uof.ProdutoRepository.GetProdutos(produtosParameters))
                .ReturnsAsync(produtos);
            _mockMapper.Setup(m => m.Map<List<ProdutoDTO>>(It.IsAny<List<Produto>>()))
                .Returns(produtosDTO);

            var mockHttpContext = new Mock<HttpContext>();
            var response = new DefaultHttpContext().Response;
            mockHttpContext.Setup(_ => _.Response).Returns(response);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await _controller.GetTodosProdutos(produtosParameters);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProdutoDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<ProdutoDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("Produto C", returnValue[0].Nome);
            Assert.Equal(10.0M, returnValue[0].Preco);
            Assert.Equal("Produto A", returnValue[1].Nome);
            Assert.Equal(5.0M, returnValue[1].Preco);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetTodosProdutos_LancaExcecao_DeveRetornarStatusCode500()
        {
            // Arrange
            var produtosParameters = new ProdutosParameters();
            _mockUof.Setup(uof => uof.ProdutoRepository.GetProdutos(produtosParameters))
                .ThrowsAsync(new Exception("Erro ao acessar o banco de dados"));

            // Act
            Func<Task> act = async () => await _controller.GetTodosProdutos(produtosParameters);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            Assert.Equal("Ocorreu um erro ao buscar os produtos.", exception.Message);
        }

        [Fact]
        public async Task GetProduto_WithValidId_DeveRetornarProdutoPorID()
        {
            // Arrange
            
            var produto = new Produto { ProdutoId = 1, Nome = "Teste Produto" };
            var produtoDTO = new ProdutoDTO { ProdutoId = 1, Nome = "Teste Produto" };

            _mockUof.Setup(uof => uof.ProdutoRepository.GetById(It.IsAny<Expression<Func<Produto, bool>>>()))
                .ReturnsAsync(produto);
            _mockMapper.Setup(m => m.Map<ProdutoDTO>(It.IsAny<Produto>()))
                .Returns(produtoDTO);           

            // Act
            var result = await _controller.GetProduto(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(produtoDTO, options => options.ComparingByMembers<ProdutoDTO>());
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeOfType<ProdutoDTO>();
            okResult.Value.As<ProdutoDTO>().ProdutoId.Should().Be(1);
            okResult.Value.As<ProdutoDTO>().Nome.Should().Be("Teste Produto");
        }
        
        [Fact]
        public async Task GetProduto_WithInvalidId_DeveRetornarNotFound()
        {
            // Arrange
            int invalidId = -1;
            _mockUof.Setup(uof => uof.ProdutoRepository.GetById(It.IsAny<Expression<Func<Produto, bool>>>()))
                .ReturnsAsync((Produto)null);          

            // Act
            var result = await _controller.GetProduto(invalidId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);            
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("Produto não encontrado");
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(99999)]
        public async Task GetProduto_WithInvalidId_DeveRetornarNotFoundQuandoIdNaoExiste(int invalidId)
        {
            // Arrange
            var produto = new Produto { ProdutoId = 1, Nome = "Teste Produto" };            

            _mockUof.Setup(uof => uof.ProdutoRepository.GetById(It.IsAny<Expression<Func<Produto, bool>>>()))
                .ReturnsAsync(produto);

            // Act
            var result = await _controller.GetProduto(invalidId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("Produto não encontrado");
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetProduto_ThrowsException()
        {
            // Arrange
            var produtoId = 1;
            _mockUof.Setup(repo => repo.ProdutoRepository.GetById(It.IsAny<Expression<Func<Produto, bool>>>()))
                    .ThrowsAsync(new Exception("Erro ao acessar o banco de dados"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetProduto(produtoId));
            Assert.Equal("Ocorreu um erro ao buscar o produto pelo seu identificador.", exception.Message);
        }      

        [Fact]
        public async Task AddProduto_ReturnsCreatedAtRouteResult_QuandoProdutoEhValido()
        {
            // Arrange
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Teste Produto", Preco = 10.0M };
            var produto = new Produto { ProdutoId = 1, Nome = "Teste Produto", Preco = 10.0M };

            _mockMapper.Setup(m => m.Map<Produto>(produtoDto)).Returns(produto);
            _mockMapper.Setup(m => m.Map<ProdutoDTO>(produto)).Returns(produtoDto);
            _mockUof.Setup(uof => uof.ProdutoRepository.Add(produto));
            _mockUof.Setup(uof => uof.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddProduto(produtoDto);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            createdAtRouteResult.RouteName.Should().Be("ObterProduto");
            createdAtRouteResult.RouteValues.Should().NotBeNull();
            createdAtRouteResult.RouteValues["id"].Should().Be(1);
            createdAtRouteResult.Value.Should().BeEquivalentTo(produtoDto);
            createdAtRouteResult.StatusCode.Should().Be(201);           
        }

        [Fact]
        public async Task AddProduto_ReturnsBadRequest_QuandoProdutoEhNulo()
        {
            // Act
            var result = await _controller.AddProduto(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, (result as BadRequestResult).StatusCode);
            result.Should().BeOfType<BadRequestResult>();
            result.As<BadRequestResult>().StatusCode.Should().Be(400);           
        }

        [Fact]
        public async Task AddProduto_ThrowsException()
        {
            // Arrange
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Teste Produto", Preco = 10.0M };

            _mockMapper.Setup(m => m.Map<Produto>(produtoDto)).Throws(new Exception("Erro ao mapear ProdutoDTO para Produto"));
            _mockMapper.Setup(m => m.Map<ProdutoDTO>(It.IsAny<Produto>())).Returns(produtoDto);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.AddProduto(produtoDto));
            Assert.Equal("Ocorreu um erro ao salvar o produto.", exception.Message);
        }

        [Fact]
        public async Task AtualizaProduto_ReturnsOk_QuandoProdutoEhAtualizado()
        {
            // Arrange
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Produto Atualizado", Preco = 20.0M };
            var produto = new Produto { ProdutoId = 1, Nome = "Produto Original", Preco = 15.0M };

            _mockMapper.Setup(m => m.Map<Produto>(produtoDto)).Returns(produto);
            _mockUof.Setup(uof => uof.ProdutoRepository.Update(produto));
            _mockUof.Setup(uof => uof.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AtualizaProduto(produtoDto.ProdutoId, produtoDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal($"Nome do produto atualizado: {produto.Nome}", okResult.Value);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task AtualizaProduto_ReturnsBadRequest_QuandoIdEhInvalido()
        {
            // Arrange
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Produto Atualizado", Preco = 20.0M };

            // Act
            var result = await _controller.AtualizaProduto(2, produtoDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, (result as BadRequestResult).StatusCode);            
        }

        [Fact]
        public async Task AtualizaProduto_ThrowsException_QuandoAtuaalizaçãoFalhar()
        {
            // Arrange
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Produto Atualizado", Preco = 20.0M };
            var produto = new Produto { ProdutoId = 1, Nome = "Produto Original", Preco = 15.0M };

            _mockMapper.Setup(m => m.Map<Produto>(produtoDto)).Returns(produto);
            _mockUof.Setup(uof => uof.ProdutoRepository.Update(produto));
            _mockUof.Setup(uof => uof.Commit()).ThrowsAsync(new Exception("Erro ao acessar o banco de dados"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.AtualizaProduto(produtoDto.ProdutoId, produtoDto));
        }

        [Fact]
        public async Task DeletaProduto_ProdutoExiste_DeveRetornarOkComNomeDoProduto()
        {
            // Arrange
            var produtoId = 1;
            var produto = new Produto { ProdutoId = produtoId, Nome = "Produto Teste" };
            var produtoDto = new ProdutoDTO { ProdutoId = produtoId, Nome = "Produto Teste" };

            _mockUof.Setup(uof => uof.ProdutoRepository.GetById(It.IsAny<Expression<Func<Produto, bool>>>()))
                .ReturnsAsync(produto);
            _mockUof.Setup(uof => uof.ProdutoRepository.Delete(produto));
            _mockUof.Setup(uof => uof.Commit()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<ProdutoDTO>(produto)).Returns(produtoDto);

            // Act
            var result = await _controller.DeletaProduto(produtoId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().Be($"Nome do produto deletado: {produtoDto.Nome}");
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DeletaProduto_ProdutoNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var produtoId = 22;
            var produto = new Produto { ProdutoId = 1, Nome = "Produto Teste" };
            var produtoDto = new ProdutoDTO { ProdutoId = 1, Nome = "Produto Teste" };

            _mockUof.Setup(uof => uof.ProdutoRepository.GetById(It.IsAny<Expression<Func<Produto, bool>>>()))
                .ReturnsAsync(produto);
            _mockUof.Setup(uof => uof.ProdutoRepository.Delete(produto));
            _mockUof.Setup(uof => uof.Commit()).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<ProdutoDTO>(produto)).Returns(produtoDto);

            // Act
            var result = await _controller.DeletaProduto(produtoId);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = result.Result as NotFoundObjectResult;
            notFoundResult.Value.Should().Be("Produto não localizado...");
            notFoundResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task DeletaProduto_ThrowsException_DeveLancarInvalidOperationException()
        {
            // Arrange
            var produtoId = 1;
            _mockUof.Setup(uof => uof.ProdutoRepository.GetById(It.IsAny<Expression<Func<Produto, bool>>>()))
                .ThrowsAsync(new Exception("Erro ao acessar o banco de dados"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.DeletaProduto(produtoId));
        }

    }
}
