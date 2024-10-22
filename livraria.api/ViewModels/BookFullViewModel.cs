using livraria.api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace livraria.api.ViewModels
{
    public class BookFullViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "O titulo é obrigatório.")]
        [StringLength(90, ErrorMessage = "Deve ter no máximo {1} caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "o Genero é obrigatório.")]
        public Guid GenreId { get; set; }
        [ForeignKey("GenreId")]
        public virtual GenreViewModel? Genre { get; set; }

        public IEnumerable<AuthorViewModel> Authors { get; set; }

    }
}
