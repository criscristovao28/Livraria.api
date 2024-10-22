using livraria.api.Enums;

namespace livraria.api.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Book> Books { get; set; }
    }
}
