using System.ComponentModel.DataAnnotations;

namespace livraria.api.ViewModels
{
    public class GenreViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [StringLength(20, ErrorMessage = "A categoria deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
