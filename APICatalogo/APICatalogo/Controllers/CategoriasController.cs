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

        [HttpGet("categoriaComProduto")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasComProdutos()
        {
            try
            {
                _logger.LogInformation("\n======== Get => categorias/categoriaComProduto ========\n");

                //return _uof.Categorias.AsNoTracking().Include(c=> c.Produtos).ToList();
                var categorias = _uof.CategoriaRepository.GetProdutosPorCategoria().ToList();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDTO;
            }
            catch (Exception)
            {
                _logger.LogInformation("\n======== Get => categorias/categoriaComProduto - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao buscar as categorias com produtos.");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> GetTodasCategoria([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                _logger.LogInformation("\n======== Get => categorias ========\n");

                var categorias = _uof.CategoriaRepository.GetCategorias(categoriasParameters);

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

                if (categorias is null)
                {
                    return NotFound("As categorias não foram encontradas");
                }
                return categoriasDTO;
            }
            catch (Exception)
            {
                _logger.LogInformation("\n======== Get => categorias - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao buscar as categorias.");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> GetCategoria(int id)
        {
            try
            {
                string messagem = $"\n======== Get => categorias/id = {id} ========\n";
                _logger.LogInformation(messagem);

                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                
                if (categoria is null)
                {
                    _logger.LogInformation($"\n======== Get => categorias/id = {id} - IS NULL ========\n");
                    return NotFound("Categoria não encontrada");                    
                }

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
                return categoriaDTO;
            }
            catch (Exception)
            {
                _logger.LogInformation($"\n======== Get => categorias/id = {id} - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao buscar a categoria pelo seu identificador.");
            }
        }

        [HttpPost]
        public ActionResult AddCategoria(CategoriaDTO categoriaDto)
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
                _uof.Commit();

                var categoriaDTO = _mapper.Map<Categoria>(categoriaDto);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
                _logger.LogInformation("\n======== Post => categorias - EXCEPTION ========\n");
                throw new Exception("Ocorreu um erro ao salvar a categoria.");
            }
        }

        [HttpPut("{id:int:min(1)}")]
        public ActionResult AtualizaCategoria(int id, CategoriaDTO categoriaDto)
        {
            try
            {
                if (id != categoriaDto.CategoriaId)
                {
                    _logger.LogInformation($"\n======== Put => categorias/id = {id} ======== - BAD REQUEST\n");
                    return BadRequest();
                }

                _logger.LogInformation("\n======== Put => categorias ========\n");

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria); 
                _uof.Commit();

                return Ok($"Nome do produto atualizado: {categoria.Nome}");
            }
            catch (Exception)
            {
                _logger.LogInformation("\n======== Put => categorias ======== - EXCEPTION\n");
                throw new Exception("Ocorreu um erro ao atualizar a categoria.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> DeletaCategoria(int id)
        {
            try
            {
                _logger.LogInformation($"\n======== Delete => categorias/id = {id} ========\n");

                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    _logger.LogInformation($"\n======== Delete => categorias/id = {id} ======== - IS NULL\n");
                    return NotFound("Categoria não localizado...");
                }

                _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok($"A categoria deletada: {categoriaDto.Nome}");
            }
            catch (Exception)
            {
                _logger.LogInformation($"\n======== Delete => categorias/id = {id} ======== - EXCEPTION\n");
                throw new Exception("Ocorreu um erro ao tratar a sua solicitação.");
            }
        }
    }
}
