using livraria.api.Data.IRepositories;
using livraria.api.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace livraria.api.Data.Repositories
{
    public class AuthorsRepository : BaseRepository<Author>, IAuthorsRepository
    {
        public AuthorsRepository(LibraryContext context) : base(context)
        {
        }

        public override async Task<Author> GetById(Guid Id)
        {
            var item = await Context.Authors
               .Where(q => q.Id == Id).FirstOrDefaultAsync();

            if(item!=null)
            {
                var bookautores = await Context.BooksAuthors.Where(q => q.AuthorId == item.Id).ToListAsync();
                var livros = new List<Book>();

                foreach (var book in bookautores)
                {
                    var livro = await Context.Books.Where(q => q.Id == book.BookId).FirstOrDefaultAsync();
                    if (livro != null)
                    {
                        livro.Genre = await Context.Genres.Where(q => q.Id == livro.GenreId).FirstOrDefaultAsync();
                        livros.Add(livro);
                    }
                }

                item.Books = livros;
            }

            return item;
        }

        public override async Task<IEnumerable<Author>> Get(int size, int page)
        {
            var retorno = new List<Author>();
            var items = await Context.Authors.Skip(page * size).Take(size).ToListAsync();

            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    var autor = await GetById(item.Id);
                    retorno.Add(autor);
                }
            }
            return retorno;
        }

        public override async Task<IEnumerable<Author>> Get()
        {
            var retorno = new List<Author>();
            var items = await Context.Authors.ToListAsync();

            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    var autor = await GetById(item.Id);
                    retorno.Add(autor);
                }
            }
            return retorno;
        }

    }
}
