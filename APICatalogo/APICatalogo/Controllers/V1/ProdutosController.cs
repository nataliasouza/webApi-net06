namespace APICatalogo.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [ApiVersion("1.0")]    
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProdutosController(IUnitOfWork context, IMapper mapper,
            ILogger<ProdutosController> logger)
        {
            _uof = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém os produtos ordenados por preço na ordem ascendente
        /// </summary>
        /// <returns>Lista de objetos Produtos ordenados por preço</returns>
        
        [HttpGet("menorpreco"), MapToApiVersion("1.0")]
        [ServiceFilter(typeof(LoggingFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutosPrecos()
        {
            try
            {
                _logger.LogInformation("\n======== Get => produtos/menorpreco ========\n");

                var produtos = await _uof.ProdutoRepository.GetProdutosPorPreco(); 
                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

                return Ok(produtosDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"\n======== Get => produtos/menorpreco - EXCEPTION ========\n");
                throw new InvalidOperationException("Ocorreu um erro ao buscar a lista de produto ordenada com o menor preço.");
            }
        }

        /// <summary>
        /// Exibe uma relação dos produtos
        /// </summary>
        /// <returns>Retorna uma lista de objetos Produto</returns>
        // api/produtos

        [HttpGet, MapToApiVersion("1.0")]
        [ServiceFilter(typeof(LoggingFilter))]       
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetTodosProdutos([FromQuery] ProdutosParameters produtosParameters)
        {
            try
            {
                _logger.LogInformation("\n======== Get => produtos/ ========\n");                

                var produtos = await _uof.ProdutoRepository.GetProdutos(produtosParameters);

                var metadata = new
                {
                    produtos.TotalCount,
                    produtos.PageSize,
                    produtos.CurrentPage,
                    produtos.TotalPages,
                    produtos.HasNext,
                    produtos.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);              

                return Ok(produtosDTO);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"\n======== Get => produtos/ - EXCEPTION ========\n");
                throw new InvalidOperationException ("Ocorreu um erro ao buscar os produtos.");
            }
        }

        /// <summary>
        /// Obtem um produto pelo seu identificador produtoId
        /// </summary>
        /// <param name="id">Código do produto</param>
        /// <returns>Um objeto Produto</returns>
        // api/produtos/1

        [ServiceFilter(typeof(LoggingFilter))]
        [HttpGet("{id:int:min(1)}", Name ="ObterProduto"), MapToApiVersion("1.0")]       
        public async Task<ActionResult<ProdutoDTO>> GetProduto(int id)
        {
            try
            {
                string messagem = $"\n======== Get => produtos/id = {id} ========\n";                
                _logger.LogInformation(messagem);
                

                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                
                if (produto is null || produto.ProdutoId != id)
                {
                    _logger.LogInformation("\n======== Get => produtos/id = {Id} - É nulo ou não existe.\n", id);
                    return NotFound("Produto não encontrado");
                }

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
                return Ok(produtoDTO);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"\n======== Get => produtos/id = {Id} - EXCEPTION ========\n", id);
                throw new InvalidOperationException("Ocorreu um erro ao buscar o produto pelo seu identificador.");
            }
        }


        /// <summary>
        /// Inclui um novo produto
        /// </summary>
        /// <param name="produtoDto">objeto Produto</param>
        /// <returns>O objeto Produto incluído</returns>

        [ServiceFilter(typeof(LoggingFilter)), MapToApiVersion("1.0")]
        [HttpPost, MapToApiVersion("1.0")]
        public async Task<ActionResult> AddProduto(ProdutoDTO produtoDto)
        {
            try
            {
                if (produtoDto is null)
                {
                    _logger.LogInformation("\n======== Post => produtos ======== - IS NULL\n");
                    return BadRequest();
                }

                _logger.LogInformation("\n======== Post => produtos ======== \n");

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Add(produto);
                await _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, produtoDTO);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex,"\n======== Post => produtos ======== - EXCEPTION\n");
                throw new InvalidOperationException("Ocorreu um erro ao salvar o produto.");
            }
        }

        /// <summary>
        /// Atualiza um produto pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="produtoDto"></param>
        /// <returns></returns>

        [ServiceFilter(typeof(LoggingFilter))]
        [HttpPut("{id:int}"), MapToApiVersion("1.0")]
        public async Task<ActionResult> AtualizaProduto(int id, ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    _logger.LogInformation("\n======== Put => produtos/id = {Id} ======== - BAD REQUEST\n", id);
                    return BadRequest();
                }

                _logger.LogInformation("\n======== Put => produtos/id = {Id} ======== \n", id);

                var produto = _mapper.Map<Produto>(produtoDto) ;

                _uof.ProdutoRepository.Update(produto);
                await _uof.Commit();

                return Ok($"Nome do produto atualizado: {produto.Nome}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "\n======== Put => produtos/id = {Id} ======== - EXCEPTION\n", id);
                throw new InvalidOperationException("Ocorreu um erro ao tratar a sua solicitação.");
            }
        }
        /// <summary>
        /// Deleta um produto pelo id
        /// </summary>
        /// <param name="id"></param>        
        /// <returns></returns>
        /// 
        [ServiceFilter(typeof(LoggingFilter))]
        [HttpDelete ("{id:int}"), MapToApiVersion("1.0")]
        public async Task<ActionResult<ProdutoDTO>> DeletaProduto(int id)
        {
            try
            {
                _logger.LogInformation("\n======== Delete => produtos/id {Id} ========\n", id);

                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto is null || produto.ProdutoId != id)
                {
                    _logger.LogInformation("\n======== Delete => produtos/id = {Id} ======== - É nulo ou não existe.\n", id);
                    return NotFound("Produto não localizado...");
                }

                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return Ok($"Nome do produto deletado: {produtoDto.Nome}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "\n======== Delete => categorias/id = {Id} ======== - EXCEPTION\n", id);
                throw new InvalidOperationException("Ocorreu um erro ao deletar um produto.");
            }
        }
    }
}
