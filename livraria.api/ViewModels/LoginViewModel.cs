using System.ComponentModel.DataAnnotations;

namespace livraria.api.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o Email ou Numero de Telefone")]
        public string EmailPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Palavra passe é obrigatório.")]
        public string Password { get; set; } = string.Empty;
    }
}
