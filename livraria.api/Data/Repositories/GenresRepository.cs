using livraria.api.Data.IRepositories;
using livraria.api.Models;

namespace livraria.api.Data.Repositories
{
    public class GenresRepository : BaseRepository<Genre>, IGenresRepository
    {
        public GenresRepository(LibraryContext context) : base(context)
        {
        }

    }
}
