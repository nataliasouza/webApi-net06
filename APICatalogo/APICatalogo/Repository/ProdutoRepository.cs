using APICatalogo.Data;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.interfaces;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            return GetAll()
                .OrderBy(on => on.Nome)
                .Skip((produtosParameters.PageNumber -1) * produtosParameters.PageSize)
                .Take(produtosParameters.PageSize)
                .ToList();
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return GetAll().OrderBy(c => c.Preco).ToList();
        }
    }
}
