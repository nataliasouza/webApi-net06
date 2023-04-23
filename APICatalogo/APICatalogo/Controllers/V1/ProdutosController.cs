using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiVersion("1.0")]    
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProdutosController(IUnitOfWork context, IMapper mapper,
            ILogger<CategoriasController> logger)
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

                return produtosDTO;
            }
            catch (Exception)
            {
                _logger.LogInformation("\n======== Get => produtos/menorpreco - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao buscar a lista de produto ordenada com o menor preço.");
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

                if (produtos is null)
                {
                    _logger.LogInformation("\n======== Get => produtos/ - IS NULL ========\n");
                    return NotFound("Produtos não encontrados");
                }

                return produtosDTO;
            }
            catch (Exception)
            {
                _logger.LogInformation("\n======== Get => produtos/ - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao buscar os produtos.");
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
                
                if (produto is null)
                {
                    _logger.LogInformation($"\n======== Get => produtos/id = {id} - IS NULL ========\n");
                    return NotFound("Produto não encontrado");
                }

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
                return produtoDTO;
            }
            catch (Exception)
            {
                _logger.LogInformation($"\n======== Get => produtos/id = {id} - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao buscar o produto pelo seu identificador.");
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
            catch (Exception)
            {
                _logger.LogInformation("\n======== Post => produtos ======== - EXCEPTION\n");
                throw new Exception("Ocorreu um erro ao salvar o produto.");
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
                    _logger.LogInformation($"\n======== Put => produtos/id = {id} ======== - BAD REQUEST\n");
                    return BadRequest();
                }

                _logger.LogInformation($"\n======== Put => produtos/id = {id} ======== \n");

                var produto = _mapper.Map<Produto>(produtoDto) ;

                _uof.ProdutoRepository.Update(produto);
                await _uof.Commit();

                return Ok($"Nome do produto atualizado: {produto.Nome}");
            }
            catch (Exception)
            {
                _logger.LogInformation($"\n======== Put => produtos/id = {id} ======== - EXCEPTION\n");
                throw new Exception("Ocorreu um erro ao tratar a sua solicitação.");
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
                _logger.LogInformation($"\n======== Delete => produtos/id {id} ========\n");

                var produto = await _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto is null)
                {
                    _logger.LogInformation($"\n======== Delete => produtos/id = {id} ======== - IS NULL\n");
                    return NotFound("Produto não localizado...");
                }

                _uof.ProdutoRepository.Delete(produto);
                await _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return Ok($"Nome do produto deletado: {produtoDto.Nome}");
            }
            catch (Exception)
            {
                _logger.LogInformation($"\n======== Delete => categorias/id = {id} ======== - EXCEPTION\n");
                throw new Exception("Ocorreu um erro ao deletar um produto.");
            }
        }
    }
}
