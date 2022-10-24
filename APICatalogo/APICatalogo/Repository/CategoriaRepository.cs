using APICatalogo.Data;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public PagedList<Categoria> GetCategorias(CategoriasParameters categoriaParameters)
        {
            return PagedList<Categoria>.ToPagedList(GetAll().OrderBy(on => on.Nome),
                               categoriaParameters.PageNumber,
                               categoriaParameters.PageSize);
        }

        public IEnumerable<Categoria> GetProdutosPorCategoria()
        {
            return GetAll().Include(x => x.Produtos);
        }
    }
}
