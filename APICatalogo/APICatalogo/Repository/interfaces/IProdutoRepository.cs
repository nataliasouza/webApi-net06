using APICatalogo.Models;

namespace APICatalogo.Repository.interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
