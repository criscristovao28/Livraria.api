using System.ComponentModel.DataAnnotations.Schema;

namespace livraria.api.Models
{
    public class BookAuthor
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public Guid AuthorId { get; set; }

        [ForeignKey("BookId")]
        public virtual Book? Book { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author? Author { get; set; }
    }
}
