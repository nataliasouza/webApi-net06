using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategorias(CategoriasParameters categoriasParameters);
        IEnumerable<Categoria> GetProdutosPorCategoria();
    }
}
