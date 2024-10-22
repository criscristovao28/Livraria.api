using livraria.api.Data.IRepositories;
using livraria.api.Models;
using Microsoft.EntityFrameworkCore;

namespace livraria.api.Data.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        public UsersRepository(LibraryContext context) : base(context)
        {

        }

        public async Task<bool> ExisteByMail(string email)
        {
            var retorno = await Context.Set<User>().Where(q => q.Email == email).FirstOrDefaultAsync();

            if (retorno == null) return false; else return true;
        }

        public async Task<bool> ExisteByPhoneNumber(string phoneNumber)
        {
            var retorno = await Context.Set<User>().Where(q => q.PhoneNumber == phoneNumber).FirstOrDefaultAsync();

            if (retorno == null) return false; else return true;
        }
    }
}
