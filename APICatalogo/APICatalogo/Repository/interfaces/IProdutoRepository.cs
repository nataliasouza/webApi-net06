using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
