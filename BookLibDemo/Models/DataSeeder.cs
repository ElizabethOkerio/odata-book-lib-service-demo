using System.Collections.Generic;
using System.Linq;

namespace BookLibService.Models
{
    public static class DataSeeder
    {
        public static void SeedDatabase(BookLibDbContext db)
        {
            db.Database.Delete();
            db.Database.CreateIfNotExists();
            var a1 = new Author { Id = 1, Name = "H" };
            var a2 = new Author { Id = 2, Name = "J" };
            var a3 = new Author { Id = 3, Name = "A3" };
            db.Authors.Add(a1);
            db.Authors.Add(a2);
            db.Authors.Add(a3);

            var b1 = new Book { Id = 1, Title = "B1" };
            var b2 = new Book { Id = 2, Title = "B2" };
            db.Books.Add(b1);
            db.Books.Add(b2);


            var u1 = new User { Id = 1, Name = "U1" };
            var u2 = new User { Id = 2, Name = "U2" };
            var u3 = new User { Id = 3, Name = "U3" };
            db.Users.Add(u1);
            db.Users.Add(u2);
            db.Users.Add(u3);

            db.SaveChanges();

            a1.Books = new List<Book>() { b1 };
            a2.Books = new List<Book>() { b1 };
            a3.Books = new List<Book>() { b2 };
            b1.Authors = new List<Author> { a1, a2 };
            b2.Authors = new List<Author> { a3 };

            u1.BooksRead = new List<Book>() { b1, b2 };
            u2.BooksRead = new List<Book>() { b1 };
            u3.BooksRead = new List<Book>() { b2 };
            u1.Friends = new List<User>() { u2, u3 };
            u2.Friends = new List<User>() { u1 };
            u3.Friends = new List<User>() { u1 };
            b1.Readers = new List<User>() { u1, u2 };
            b2.Readers = new List<User>() { u1, u3 };

            db.SaveChanges();
        }
    }
}