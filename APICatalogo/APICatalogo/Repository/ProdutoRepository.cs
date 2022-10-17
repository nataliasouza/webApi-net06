using APICatalogo.Data;
using APICatalogo.Models;
using APICatalogo.Repository.interfaces;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return GetAll().OrderBy(c => c.Preco).ToList();
        }
    }
}
