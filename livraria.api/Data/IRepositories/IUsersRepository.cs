using livraria.api.Models;

namespace livraria.api.Data.IRepositories
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<bool> ExisteByMail(string phoneNumber);
        Task<bool> ExisteByPhoneNumber(string phoneNumber);
    }
}
