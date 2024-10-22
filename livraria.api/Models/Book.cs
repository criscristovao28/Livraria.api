using System.ComponentModel.DataAnnotations.Schema;

namespace livraria.api.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid GenreId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Genre? Genre { get; set; }
        public virtual IEnumerable<Author> Authors { get; set; }
    }
}
