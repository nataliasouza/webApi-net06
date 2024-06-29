namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedList<Categoria>> GetTodasCategoriasRepository(CategoriasParameters categoriasParameters)
        {
            return await PagedList<Categoria>.ToPagedList(GetAll().OrderBy(on => on.Nome),
                               categoriasParameters.PageNumber,
                               categoriasParameters.PageSize);
        }
        public async Task<IEnumerable<Categoria>> GetCategoriasComProdudosRepository()
        {
            return await GetAll().Include(x => x.Produtos).ToListAsync();
        }
    }
}
