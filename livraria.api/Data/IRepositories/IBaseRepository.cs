namespace livraria.api.Data.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> GetById(Guid Id);
        Task<T> Update(T entity);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<T>> Get();
        Task<IEnumerable<T>> Get(int tamanho, int pagina);
    }
}
