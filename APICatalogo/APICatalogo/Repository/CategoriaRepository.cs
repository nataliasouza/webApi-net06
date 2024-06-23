namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Categoria>> GetTodasCategoriasRepository(CategoriasParameters categoriaParameters)
        {
            return await PagedList<Categoria>.ToPagedList(GetAll().OrderBy(on => on.Nome),
                               categoriaParameters.PageNumber,
                               categoriaParameters.PageSize);
        }
        public async Task<IEnumerable<Categoria>> GetCategoriasComProdudosRepository()
        {
            return await GetAll().Include(x => x.Produtos).ToListAsync();
        }
    }
}
