using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategorias(CategoriasParameters categoriasParameters);
        Task <IEnumerable<Categoria>> GetProdutosPorCategoria();
    }
}
