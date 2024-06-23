namespace APICatalogo.Repository.interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetTodasCategoriasRepository(CategoriasParameters categoriasParameters);
        Task <IEnumerable<Categoria>> GetCategoriasComProdudosRepository();
    }
}
