using System.ComponentModel.DataAnnotations;

namespace livraria.api.ViewModels
{
    public class AuthorViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório.")]
        [StringLength(90, ErrorMessage = "A categoria deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
