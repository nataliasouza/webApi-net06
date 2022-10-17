using APICatalogo.Models;

namespace APICatalogo.Repository.interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<Categoria> GetProdutosPorCategoria();
    }
}
