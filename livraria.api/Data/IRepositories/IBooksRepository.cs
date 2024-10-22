using livraria.api.Models;
using livraria.api.ViewModels;

namespace livraria.api.Data.IRepositories
{
    public interface IBooksRepository : IBaseRepository<Book>
    {
        Task<Book> Add(BookViewModel entity);
    }
}
