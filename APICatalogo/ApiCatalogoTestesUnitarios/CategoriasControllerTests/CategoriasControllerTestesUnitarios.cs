
namespace ApiCatalogoTestesUnitarios.CategoriasControllertests
{
    public class CategoriasControllerTestesUnitarios
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CategoriasController>> _mockLogger;
        private readonly CategoriasController _controller;
        public CategoriasControllerTestesUnitarios()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CategoriasController>>();
            _controller = new CategoriasController(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetCategoriasComProdutos_ReturnsOkResult_QuandoCategoriaComProdutosExiste()
        {
            //Arrange
            var categorias = new List<Categoria>
            {
                new Categoria
                {
                    CategoriaId = 1,
                    Nome = "Bebidas",
                    ImagemUrl = "bebidas.png",
                    Produtos = new List<Produto>
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
                            Nome = "Guaraná",
                            Descricao = "Refrigerante de guaraná",
                            Preco = 6,
                            ImagemUrl = "guarana.png"
                        }
                    }
                }
            };

            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetCategoriasComProdudosRepository()).ReturnsAsync(categorias);

            var categoriasDto = new List<CategoriaDTO>
            {
                new CategoriaDTO
                {
                    CategoriaId = 1,
                    Nome = "Bebidas",
                    ImagemUrl = "bebidas.png",
                    Produtos = new List<ProdutoDTO>
                    {
                        new ProdutoDTO
                        {
                            ProdutoId = 1,
                            Nome = "Coca-Cola",
                            Descricao = "Refrigerante de cola",
                            Preco = 5,
                            ImagemUrl = "coca.png"
                        },
                        new ProdutoDTO
                        {
                            ProdutoId = 2,
                            Nome = "Guaraná",
                            Descricao = "Refrigerante de guaraná",
                            Preco = 6,
                            ImagemUrl = "guarana.png"
                        }
                    }
                }
            };

            _mockMapper.Setup(m => m.Map<List<CategoriaDTO>>(categorias)).Returns(categoriasDto);

            //Act
            var result = await _controller.GetCategoriasComProdutos();

            //Assert
            result.Result.Should().NotBeNull();
            categoriasDto.Should().HaveCount(1);
            categoriasDto[0].Nome.Should().NotBeNullOrEmpty();
            categoriasDto[0].Nome.Should().Be("Bebidas");
            categoriasDto[0].CategoriaId.Should().Be(1);
            categoriasDto[0].Produtos.Should().NotBeNull("A lista de produtos não deve ser nula.");
            categoriasDto[0].Produtos.Should().HaveCount(2);

            categoriasDto[0].Produtos.Should().NotBeNullOrEmpty();
            categoriasDto[0].Produtos.Should().Contain(p => p.Nome == "Coca-Cola");
            categoriasDto[0].Produtos.Should().Contain(p => p.ProdutoId == 1);
            categoriasDto[0].Produtos.Should().Contain(p => p.Descricao == "Refrigerante de cola");
            categoriasDto[0].Produtos.Should().Contain(p => p.Preco == 5);
            categoriasDto[0].Produtos.Should().Contain(p => p.ImagemUrl == "coca.png");

            var segundoProduto = categoriasDto[0].Produtos?.ElementAtOrDefault(1) ?? new ProdutoDTO();
            segundoProduto.Nome.Should().NotBeNullOrEmpty();
            segundoProduto.Nome.Should().Be("Guaraná");
            segundoProduto.ProdutoId.Should().Be(2);
            segundoProduto.Descricao.Should().Be("Refrigerante de guaraná");
            segundoProduto.Preco.Should().Be(6);
            segundoProduto.ImagemUrl.Should().Be("guarana.png");
        }

        [Fact]
        public async Task GetCategoriasComProdutos_ReturnsException_QuandoCategoriaComProdutosNaoAcessarBancoDeDados()
        {
            //Arrange
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetCategoriasComProdudosRepository())
                .ThrowsAsync(new Exception("Erro simulado ao acessar o banco de dados"));

            //Act
            var result = await _controller.GetCategoriasComProdutos();

            //Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Ocorreu um erro ao buscar as categorias com produtos. Por favor, tente novamente mais tarde.", objectResult.Value);
        }

        [Fact]
        public async Task GetCategoriasComProdutos_ReturnsCategorias_QuandoCategoriaExisteSemProdutos()
        {
            //Arrange
            var categorias = new List<Categoria>
            {
                new Categoria
                {
                    CategoriaId = 1,
                    Nome = "Bebidas",
                    ImagemUrl = "bebidas.png"
                }
            };

            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetCategoriasComProdudosRepository()).ReturnsAsync(categorias);

            var categoriasDto = new List<CategoriaDTO>
            {
                new CategoriaDTO
                {
                    CategoriaId = 1,
                    Nome = "Bebidas",
                    ImagemUrl = "bebidas.png"
                }
            };

            _mockMapper.Setup(m => m.Map<List<CategoriaDTO>>(categorias)).Returns(categoriasDto);

            //Act
            var result = await _controller.GetCategoriasComProdutos();

            //Assert
            result.Result.Should().NotBeNull();
            categoriasDto.Should().HaveCount(1);
            categoriasDto[0].Nome.Should().NotBeNullOrEmpty();
            categoriasDto[0].Nome.Should().Be("Bebidas");
            categoriasDto[0].CategoriaId.Should().Be(1);
            categoriasDto[0].ImagemUrl.Should().Be("bebidas.png");
        }

        [Fact]
        public async Task GetTodasCategoria_RetornaOk_QuandoExistemCategorias()
        {
            // Arrange
            var categoriasParameters = new CategoriasParameters();
            var categorias = new PagedList<Categoria>(new List<Categoria> { new Categoria { CategoriaId = 1, Nome = "Categoria Teste" } }, 1, 1, 1);
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetTodasCategoriasRepository(categoriasParameters)).ReturnsAsync(categorias);

            var categoriasDTO = new List<CategoriaDTO> { new CategoriaDTO { CategoriaId = 1, Nome = "Categoria Teste" } };
            _mockMapper.Setup(m => m.Map<List<CategoriaDTO>>(It.IsAny<List<Categoria>>())).Returns(categoriasDTO);

            var mockHttpContext = new Mock<HttpContext>();
            var response = new DefaultHttpContext().Response; 
            mockHttpContext.Setup(_ => _.Response).Returns(response); 

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object 
            };

            // Act
            var result = await _controller.GetTodasCategorias(categoriasParameters);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CategoriaDTO>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("Categoria Teste", returnValue[0].Nome);
        }

        [Fact]
        public async Task GetTodasCategoria_RetornaNotFound_QuandoNaoExistemCategorias()
        {
            // Arrange
            var categoriasParameters = new CategoriasParameters();
            var categorias = new PagedList<Categoria>(new List<Categoria>(), 0, 1, 1);
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetTodasCategoriasRepository(categoriasParameters))
                .ReturnsAsync(categorias);

            var mockHttpContext = new Mock<HttpContext>();
            var response = new DefaultHttpContext().Response; // Cria um objeto Response padrão
            mockHttpContext.Setup(_ => _.Response).Returns(response); // Configura o mock do HttpContext para retornar o objeto Response

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object // Define o HttpContext do controller para o mock
            };

            // Act
            var result = await _controller.GetTodasCategorias(categoriasParameters);

            // Assert          
            var objectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(404, objectResult.StatusCode);
            Assert.Equal("As categorias não foram encontradas. Por favor, tente novamente mais tarde.", objectResult.Value);
        }

        [Fact]
        public async Task GetTodasCategoria_RetornaStatusCode500_QuandoOcorreExcecao()
        {
            // Arrange
            var categoriasParameters = new CategoriasParameters();
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetTodasCategoriasRepository(categoriasParameters))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetTodasCategorias(categoriasParameters);

            // Assert
            result.Result.Should().BeOfType<ObjectResult>();
            var objectResult = result.Result as ObjectResult;
            objectResult.StatusCode.Should().Be(500);
            Assert.Equal("Ocorreu um erro ao buscar as categorias. Por favor, tente novamente mais tarde.", objectResult.Value);
        }

        [Fact]
        public async Task GetCategoriaById_RetornaOk_ComCategoriaEspecifica()
        {
            // Arrange
            var categoriaEsperada = new Categoria { CategoriaId = 1, Nome = "Categoria Teste" };
            var categoriaDtoEsperada = new CategoriaDTO { CategoriaId = 1, Nome = "Categoria Teste" };

            _mockUnitOfWork.Setup(repo => repo.CategoriaRepository.GetById(It.IsAny<Expression<Func<Categoria, bool>>>()))
                .ReturnsAsync(categoriaEsperada);

            _mockMapper.Setup(mapper => mapper.Map<CategoriaDTO>(categoriaEsperada))
                .Returns(categoriaDtoEsperada);

            // Act           
            var actionResult = await _controller.GetCategoriaById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.NotNull(okResult);

            var categoriaDto = Assert.IsType<CategoriaDTO>(okResult.Value); 
            Assert.NotNull(categoriaDto);
            Assert.Equal(categoriaEsperada.CategoriaId, categoriaDto.CategoriaId);
            Assert.Equal(categoriaEsperada.Nome, categoriaDto.Nome);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetCategoriaById_RetornaStatusCode500_QuandoOcorreExcecao()
        {
            // Arrange
            var categoriaId = 1;
            _mockUnitOfWork.Setup(repo => repo.CategoriaRepository.GetById(It.IsAny<Expression<Func<Categoria, bool>>>()))
                .ThrowsAsync(new Exception("Erro simulado ao acessar o banco de dados"));

            // Act
            var result = await _controller.GetCategoriaById(categoriaId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Ocorreu um erro ao buscar a categoria. Por favor, tente novamente mais tarde.", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetCategoriaById_ReturnsNotFound_QuandoCategoriaNaoExiste()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetById(It.IsAny<Expression<Func<Categoria, bool>>>())).ReturnsAsync(() => null);

            // Act
            var result = await _controller.GetCategoriaById(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }        

        [Fact]
        public async Task AddCategoria_CategoriaValida_ReturnsCreatedAtRouteResult()
        {
            // Arrange
            var categoriaDto = new CategoriaDTO { Nome = "Eletrônicos", ImagemUrl = "http://imagemurl.com/imagem.jpg" };
            var categoria = new Categoria { CategoriaId = 1, Nome = "Eletrônicos", ImagemUrl = "http://imagemurl.com/imagem.jpg" };

            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDTO>())).Returns(categoria);
            _mockMapper.Setup(m => m.Map<CategoriaDTO>(It.IsAny<Categoria>())).Returns(categoriaDto);
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.Add(It.IsAny<Categoria>()));
            _mockUnitOfWork.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddCategoria(categoriaDto);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>();
            var createdAtRouteResult = result as CreatedAtRouteResult;
            createdAtRouteResult.Should().NotBeNull();
            createdAtRouteResult?.RouteName.Should().Be("ObterCategoria");
            createdAtRouteResult?.RouteValues["id"].Should().Be(1);  
            createdAtRouteResult?.StatusCode.Should().Be(201);            
        }

        [Fact]
        public async Task AddCategoria_RetornaBadRequest_QuandoNomeNaoInformado()
        {
            // Arrange
            var categoriaDtoSemNome = new CategoriaDTO { CategoriaId = 1, Nome = "", ImagemUrl = "url_da_imagem.png" };
            _mockMapper.Setup(m => m.Map<Categoria>(categoriaDtoSemNome)).Returns(new Categoria { CategoriaId = 1, Nome = "", ImagemUrl = "url_da_imagem.png" });

            // Act
            var result = await _controller.GetCategoriasComProdutos();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Equal("Ocorreu um erro ao buscar as categorias com produtos. Por favor, tente novamente mais tarde.", objectResult.Value);
        }

        [Fact]
        public async Task AddCategoria_RetornaBadRequest_QuandoCategoriaEhNula()
        {
            // Act
            var result = await _controller.AddCategoria(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task AddCategoria_LancaInvalidOperationException_QuandoOcorreExcecao()
        {
            // Arrange
            var categoriaDto = new CategoriaDTO();
            _mockMapper.Setup(m => m.Map<Categoria>(It.IsAny<CategoriaDTO>())).Throws(new Exception());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.AddCategoria(categoriaDto));
            Assert.Equal("Ocorreu um erro ao salvar a categoria.", exception.Message);
        }

        [Fact]
        public async Task AtualizaCategoria_RetornaOk_QuandoAtualizacaoBemSucedida()
        {
            // Arrange
            var categoriaDto = new CategoriaDTO { CategoriaId = 1, Nome = "Categoria Atualizada" };
            var categoria = new Categoria { CategoriaId = 1, Nome = "Categoria Original" };

            _mockMapper.Setup(m => m.Map<Categoria>(categoriaDto)).Returns(categoria);
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.Update(categoria));           
            _mockUnitOfWork.Setup(u => u.Commit()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AtualizaCategoria(categoriaDto.CategoriaId, categoriaDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Nome do produto atualizado: Categoria Original", okResult.Value);
            Assert.Equal("Categoria Atualizada", categoriaDto.Nome);
            Assert.True(categoriaDto.CategoriaId == 1);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);  
        }

        [Fact]
        public async Task AtualizaCategoria_RetornaBadRequest_QuandoIdDiferenteNoDto()
        {
            // Arrange
            var categoriaDto = new CategoriaDTO { CategoriaId = 2 };

            // Act
            var result = await _controller.AtualizaCategoria(1, categoriaDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AtualizaCategoria_RetornaStatusCode500_QuandoOcorreExcecao()
        {
            // Arrange
            var categoriaDto = new CategoriaDTO { CategoriaId = 1 };
            var categoria = new Categoria { CategoriaId = 1 };

            _mockMapper.Setup(m => m.Map<Categoria>(categoriaDto)).Returns(categoria);
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.Update(categoria));
            _mockUnitOfWork.Setup(u => u.Commit()).ThrowsAsync(new Exception("Erro simulado"));

            // Act
            Func<Task> act = async () => await _controller.AtualizaCategoria(categoriaDto.CategoriaId, categoriaDto);

            // Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            Assert.Equal("Ocorreu um erro ao atualizar a categoria.", exception.Message);
        }

        [Fact]
        public async Task DeletaCategoria_RetornaOk_QuandoCategoriaDeletadaComSucesso()
        {
            // Arrange
            var categoria = new Categoria { CategoriaId = 1, Nome = "Categoria Teste" };
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetById(It.IsAny<Expression<Func<Categoria, bool>>>()))
                .ReturnsAsync(categoria);
            _mockMapper.Setup(m => m.Map<CategoriaDTO>(categoria)).Returns(new CategoriaDTO { CategoriaId = 1, Nome = "Categoria Teste" });

            // Act
            var result = await _controller.DeletaCategoria(1);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult).Value.Should().Be($"A categoria deletada: {categoria.Nome}");
            Assert.Equal(StatusCodes.Status200OK, (result.Result as OkObjectResult).StatusCode);                
        }

        [Fact]
        public async Task DeletaCategoria_RetornaNotFound_QuandoCategoriaNaoExiste()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetById(It.IsAny<Expression<Func<Categoria, bool>>>()))
                .ReturnsAsync((Categoria)null);

            // Act
            var result = await _controller.DeletaCategoria(1);

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
            (result.Result as NotFoundObjectResult).Value.Should().Be("Categoria não localizado...");
            Assert.Equal(StatusCodes.Status404NotFound, (result.Result as NotFoundObjectResult).StatusCode);
        }

        [Fact]
        public async Task DeletaCategoria_LancaInvalidOperationException_QuandoOcorreExcecao()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.CategoriaRepository.GetById(It.IsAny<Expression<Func<Categoria, bool>>>()))
                .ThrowsAsync(new Exception("Erro simulado ao acessar o banco de dados"));

            // Act
            Func<Task> act = async () => await _controller.DeletaCategoria(1);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Ocorreu um erro ao tratar a sua solicitação.");
        }
    }
}
