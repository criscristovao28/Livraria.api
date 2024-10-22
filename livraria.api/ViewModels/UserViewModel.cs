using livraria.api.Enums;
using System.ComponentModel.DataAnnotations;

namespace livraria.api.ViewModels
{
    public class UserViewModel
    {
        public Guid? Id { get; set; }
        public bool Active { get; set; }

        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo deve ter no máximo {1} caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        [StringLength(255, ErrorMessage = "O e-mail deve ter no máximo {1} caracteres.")]
        public string Email { get; set; } = string.Empty;
        public EUserType? UserType { get; set; }
    }
}
