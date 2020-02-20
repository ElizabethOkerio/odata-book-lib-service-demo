using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibService.Models
{
    public class BookLibDbContext: DbContext
    {
        public BookLibDbContext() : base("Server=(localdb)\\mssqllocaldb;Database=BookLibDemoServiceDb;Trusted_Connection=True;MultipleActiveResultSets=true")
        {

        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Authors)
                .WithMany(a => a.Books);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany();

            modelBuilder.Entity<User>()
                .HasMany(u => u.BooksRead)
                .WithMany(b => b.Readers);
        }
    }
}
