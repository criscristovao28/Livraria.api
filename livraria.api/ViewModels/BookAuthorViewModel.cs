using livraria.api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace livraria.api.ViewModels
{
    public class BookAuthorViewModel
    {
        [Required(ErrorMessage = "O Id do livro é obrigatória.")]
        public Guid BookdId { get; set; }

        [Required(ErrorMessage = "O Id do livro é obrigatória.")]
        public Guid AuthorId { get; set; }

        [ForeignKey("BookdId")]
        public virtual BookViewModel? Book { get; set; }

        [ForeignKey("AuthorId")]
        public virtual AuthorViewModel? Author { get; set; }
    }
}
