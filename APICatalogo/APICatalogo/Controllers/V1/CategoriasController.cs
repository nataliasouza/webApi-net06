using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [ApiVersion("1.0")]    
    [Route("v{v:apiVersion}/[controller]")]
    [ApiController]
    
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CategoriasController(IUnitOfWork context, IMapper mapper,
            ILogger<CategoriasController> logger)
        {
            _uof = context;
            _mapper = mapper;
            _logger = logger;
        }
        
        [HttpGet("categoriaComProduto"), MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasComProdutos()
        {
            try
            {
                _logger.LogInformation("\n======== Get => categorias/categoriaComProduto ========\n");
                
                var categorias = await _uof.CategoriaRepository.GetProdutosPorCategoria();
                
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao buscar as categorias com produtos.");
                return StatusCode(500, "Ocorreu um erro ao buscar as categorias com produtos. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpGet, MapToApiVersion("1.0")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetTodasCategoria([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                _logger.LogInformation("\n======== Get => categorias ========\n");

                var categorias = await _uof.CategoriaRepository.GetCategorias(categoriasParameters);

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

                if (!categorias.Any())
                {
                    _logger.LogInformation("Nenhuma categoria foi encontrada.");
                    return NotFound("As categorias não foram encontradas");
                }
                return categoriasDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao buscar as categorias.");
                return StatusCode(500, "Ocorreu um erro ao buscar as categorias. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria"), MapToApiVersion("1.0")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            try
            {
                string messagem = $"\n======== Get => categorias/id = {id} ========\n";
                _logger.LogInformation(messagem);

                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    string mensagem = string.Format("\n======== Get => categorias/id = {0} - IS NULL ========\n", id);
                    _logger.LogInformation(mensagem);
                    return NotFound("Categoria não encontrada");
                }

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
                return categoriaDTO;
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("Ocorreu um erro ao buscar a categoria pelo identificador {0}.", id);
                _logger.LogError(ex, errorMessage);
                return StatusCode(500, "Ocorreu um erro ao buscar a categoria. Por favor, tente novamente mais tarde.");
            }
        }

        [HttpPost, MapToApiVersion("1.0")]
        public async Task<ActionResult> AddCategoria(CategoriaDTO categoriaDto)
        {
            try
            {
                if (categoriaDto is null)
                {
                    _logger.LogInformation("\n======== Post => categorias ======== - IS NULL\n");
                    return BadRequest();
                }

                _logger.LogInformation("\n======== Post => categorias ========\n");              

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Add(categoria);
                await _uof.Commit();

                var categoriaDTO = _mapper.Map<Categoria>(categoriaDto);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\n======== Post => categorias - EXCEPTION ========\n");
                throw new InvalidOperationException("Ocorreu um erro ao salvar a categoria.");
            }
        }

        [HttpPut("{id:int:min(1)}"), MapToApiVersion("1.0")]
        public async Task<ActionResult> AtualizaCategoria(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    _logger.LogInformation("\n======== Put => categorias/id = {Id} ======== - BAD REQUEST\n", id);
                    return BadRequest();
                }

                _logger.LogInformation("\n======== Put => categorias ========\n");

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria); 
                await _uof.Commit();

                return Ok($"Nome do produto atualizado: {categoria.Nome}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\n======== Put => categorias ======== - EXCEPTION\n");
                throw new InvalidOperationException("Ocorreu um erro ao atualizar a categoria.");
            }
        }

        [HttpDelete("{id:int}"), MapToApiVersion("1.0")]
        public async Task<ActionResult<CategoriaDTO>> DeletaCategoria(int id)
        {
            try
            {
                _logger.LogInformation("\n======== Delete => categorias/id = {Id} ========\n", id);

                var categoria = await _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    _logger.LogInformation("\n======== Delete => categorias/id = {Id} ======== - IS NULL\n", id);
                    return NotFound("Categoria não localizado...");
                }

                _uof.CategoriaRepository.Delete(categoria);
                await _uof.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok($"A categoria deletada: {categoriaDto.Nome}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "\n======== Delete => categorias/id = {Id} ======== - EXCEPTION\n", id);
                throw new InvalidOperationException("Ocorreu um erro ao tratar a sua solicitação.");
            }
        }
    }
}
