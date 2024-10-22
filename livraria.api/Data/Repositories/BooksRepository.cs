using livraria.api.Data.IRepositories;
using livraria.api.Models;
using livraria.api.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace livraria.api.Data.Repositories
{
    public class BooksRepository : BaseRepository<Book>, IBooksRepository
    {
        public BooksRepository(LibraryContext context) : base(context)
        {
        }

        public async Task<Book> Add(BookViewModel entity)
        {
            var autores = entity.Authors;
            if(autores.Count()<1) throw new Exception("O Livro deve ter pelo menos 1 autor");
            var genre = await Context.Genres.Where(q => q.Id == entity.GenreId).FirstOrDefaultAsync();
            if(genre == null) throw new Exception("Genero não encontrado");

            foreach (var author in autores)
            {
                var exist = await Context.Authors.Where(q => q.Id == Guid.Parse(author)).FirstOrDefaultAsync();
                if (exist==null)
                {
                    throw new Exception("Autor com o código " + author + "Não encontrado");
                }
            }

            var book = new Book()
            {
                Title = entity.Title,
                GenreId = entity.GenreId,
            };

            Context.Books.Add(book);
            Context.SaveChanges();

            foreach (var author in autores)
            {
                Context.BooksAuthors.Add(new BookAuthor()
                {
                    AuthorId = Guid.Parse(author),
                    BookId = book.Id,
                });
            }

            Context.SaveChanges();
            book.Genre= await Context.Genres.Where(q=> q.Id== book.GenreId).FirstOrDefaultAsync();
            return book;
        }


        public override async Task<IEnumerable<Book>> Get()
        {
            var items = await Context.Books.ToListAsync();

            foreach (var book in items)
            {
                var item = await GetById(book.Id);
                if(item!=null)
                {
                    item.Genre = book.Genre;
                    item.Authors = book.Authors;
                }               
            }
            return items;
        }

        public override async Task<Book> GetById(Guid Id)
        {
            var item = await Context.Books
               .Where(q=> q.Id==Id).FirstOrDefaultAsync();

            if (item!=null)
            {
                item.Genre = await Context.Genres.Where(q => q.Id == item.GenreId).FirstOrDefaultAsync();

                var bookautores= await Context.BooksAuthors.Where(q => q.BookId == item.Id).ToListAsync();

                var autores = new List<Author>();
                foreach (var book in bookautores)
                {
                    var autor = await Context.Authors.Where(q=> q.Id == book.AuthorId).FirstOrDefaultAsync();
                    if(autor!=null) autores.Add(autor);
                }
               
                item.Authors = autores;
            }
            return item;
        }

        public override async Task<IEnumerable<Book>> Get(int size, int page)
        {
            var items = await Context.Books.Skip(page * size).Take(size).ToListAsync();

            foreach (var book in items)
            {
                book.Genre = await Context.Genres.Where(q => q.Id == book.GenreId).FirstOrDefaultAsync();
            }

            return items;
        }

    }
}
