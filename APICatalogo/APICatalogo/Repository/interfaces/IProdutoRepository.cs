namespace APICatalogo.Repository.interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task <PagedList<Produto>> GetProdutos(ProdutosParameters produtosParameters);
        Task <IEnumerable<Produto>> GetProdutosPorPreco();
        Task<PagedList<Produto>> GetOrdenaPorNome(ProdutosParameters produtosParameters);
    }
}
