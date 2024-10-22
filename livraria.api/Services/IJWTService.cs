using livraria.api.DTO;
using livraria.api.Models;

namespace livraria.api.Services
{
    public interface IJWTService
    {
        public TokenResponseDTO GetToken(User user);
    }
}
