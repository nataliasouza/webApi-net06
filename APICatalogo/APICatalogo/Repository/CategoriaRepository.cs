using APICatalogo.Data;
using APICatalogo.Models;
using APICatalogo.Repository.interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Categoria> GetProdutosPorCategoria()
        {
            return GetAll().Include(x => x.Produtos);
        }
    }
}
