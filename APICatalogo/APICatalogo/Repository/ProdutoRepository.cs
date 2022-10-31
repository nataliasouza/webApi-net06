using APICatalogo.Data;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters)
        {
            //return GetAll()
            //    .OrderBy(on => on.Nome)
            //    .Skip((produtosParameters.PageNumber -1) * produtosParameters.PageSize)
            //    .Take(produtosParameters.PageSize)
            //    .ToList();

            return await PagedList<Produto>.ToPagedList(GetAll().OrderBy(on => on.ProdutoId),
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        public async Task <IEnumerable<Produto>> GetProdutosPorPreco()
        {
            return await GetAll().OrderBy(c => c.Preco).ToListAsync();
        }

        public async Task<PagedList<Produto>>GetOrdenaPorNome(ProdutosParameters produtosParameters)
        {
            return await PagedList<Produto>.ToPagedList(GetAll().OrderBy(on => on.Nome),
                 produtosParameters.PageNumber, produtosParameters.PageSize);                
                
        }
    }
}
