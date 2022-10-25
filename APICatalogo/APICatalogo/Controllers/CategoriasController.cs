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

        public CategoriasController(IUnitOfWork context, IMapper mapper)
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("categoriaComProduto")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasComProdutos()
        {
            try
            {
                //return _uof.Categorias.AsNoTracking().Include(c=> c.Produtos).ToList();
                var categorias = _uof.CategoriaRepository.GetProdutosPorCategoria().ToList();
                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDTO;
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao buscar as categorias com produtos.");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> GetTodasCategoria([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
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
                throw new Exception("Ocorreu um erro ao buscar as categorias.");
            }
        }

        [HttpGet("{id:int:min(1)}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> GetCategoria(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);
                
                if (categoria is null)
                {
                    return NotFound("Categoria não encontrada");
                }

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
                return categoriaDTO;
            }
            catch (Exception)
            {
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
                    return BadRequest();
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit();

                var categoriaDTO = _mapper.Map<Categoria>(categoriaDto);

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoriaDTO);
            }
            catch (Exception)
            {
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
                    return BadRequest();
                }

                var categoria = _mapper.Map<Categoria>(categoriaDto);

                _uof.CategoriaRepository.Update(categoria); 
                _uof.Commit();

                return Ok($"Nome do produto atualizado: {categoria.Nome}");
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao atualizar a categoria.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> DeletaCategoria(int id)
        {
            try
            {
                var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

                if (categoria is null)
                {
                    return NotFound("Categoria não localizado...");
                }

                _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit();

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok($"A categoria deletada: {categoriaDto.Nome}");
            }
            catch (Exception)
            {
                throw new Exception("Ocorreu um erro ao tratar a sua solicitação.");
            }
        }
    }
}
