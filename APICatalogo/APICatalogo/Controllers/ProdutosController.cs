using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPrecos()
        {
            try
            {
                var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
                var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

                return produtosDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao buscar a lista de produto ordenada com o menor preço.");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> GetTodosProdutos([FromQuery] ProdutosParameters produtosParameters)
        {
            try
            {
                var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters);

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
                    return NotFound("Produtos não encontrados");
                }

                return produtosDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Ocorreu um erro ao buscar os produtos.");
            }
        }

        [HttpGet("{id:int:min(1)}", Name ="ObterProduto")]
        public ActionResult<ProdutoDTO> GetProduto(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);
                
                if (produto is null)
                {
                    return NotFound("Produto não encontrado");
                }

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);
                return produtoDTO;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um erro ao buscar o produto pelo seu identificador.");
            }
        }

        [HttpPost]
        public ActionResult AddProduto(ProdutoDTO produtoDto)
        {
            try
            {
                if (produtoDto is null)
                {
                    return BadRequest();
                }

                var produto = _mapper.Map<Produto>(produtoDto);

                _uof.ProdutoRepository.Add(produto);
                _uof.Commit();

                var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

                return new CreatedAtRouteResult("ObterProduto",
                    new { id = produto.ProdutoId }, produtoDTO);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um erro ao salvar o produto.");
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult AtualizaProduto(int id, ProdutoDTO produtoDto)
        {
            try
            {
                if (id != produtoDto.ProdutoId)
                {
                    return BadRequest();
                }

                var produto = _mapper.Map<Produto>(produtoDto) ;

                _uof.ProdutoRepository.Update(produto);
                _uof.Commit();

                return Ok($"Nome do produto atualizado: {produto.Nome}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um erro ao tratar a sua solicitação.");
            }
        }

        [HttpDelete ("{id:int}")]
        public ActionResult<ProdutoDTO> DeletaProduto(int id)
        {
            try
            {
                var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

                if (produto is null)
                {
                    return NotFound("Produto não localizado...");
                }

                _uof.ProdutoRepository.Delete(produto);
                _uof.Commit();

                var produtoDto = _mapper.Map<ProdutoDTO>(produto);

                return Ok($"Nome do produto deletado: {produtoDto.Nome}");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                    "Ocorreu um erro ao deletar um produto.");
            }
        }
    }
}
