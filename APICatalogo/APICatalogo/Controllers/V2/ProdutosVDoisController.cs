using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Pagination;
using APICatalogo.Repository.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers.V2
{
    [ApiVersion("2.0")]    
    [Route("v{v:apiVersion}/produtos")]
    [ApiController]
    public class ProdutosVDoisController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProdutosVDoisController(IUnitOfWork context, IMapper mapper,
            ILogger<ProdutosController> logger)
        {
            _uof = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("nomeproduto")]
        [ServiceFilter(typeof(LoggingFilter))]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetNomeOrdenados([FromQuery] ProdutosParameters produtosParameters)
        {      
            try
            {
                _logger.LogInformation("\n======== Get => produtos/ ========\n");

                var produtos = await _uof.ProdutoRepository.GetOrdenaPorNome(produtosParameters);

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
                    _logger.LogInformation("\n======== Get => produtos/menorpreco - IS NULL ========\n");
                    return NotFound("Produtos não encontrados");
                }

                return produtosDTO;
            }
            catch (Exception)
            {
                _logger.LogInformation("\n======== Get => produtos/menorpreco - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao buscar os produtos.");
            }
        }

        [HttpGet("menorpreco")]
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

        [HttpGet]
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

        [ServiceFilter(typeof(LoggingFilter))]
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
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
    }
}
