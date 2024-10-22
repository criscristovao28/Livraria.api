using livraria.api.Enums;

namespace livraria.api.DTO
{
    public class TokenResponseDTO
    {
        public string? Token { get; set; }
        public DateTime ValidTo { get; set; }
        public Guid UserId { get; set; }
        public EUserType UserType { get; set; }
    }
}
