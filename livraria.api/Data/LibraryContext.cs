using livraria.api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace livraria.api.Data
{
    public class LibraryContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }

        public LibraryContext(DbContextOptions options) : base(options)
        {

        }

        public override int SaveChanges()
        {
            var DeletedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Deleted).ToList();
            DeletedEntities.ForEach(E => { E.State = EntityState.Unchanged; });

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("Id") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("Id").CurrentValue = Guid.NewGuid();
                }
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
             .Property(u => u.Id)
            .HasDefaultValueSql("newsequentialid()");

            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<string>>()
            .HasKey(x => new { x.LoginProvider, x.ProviderKey });
        }
    }
}
