using System.Collections.Generic;

namespace BookLibService.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public ICollection<Author> Authors { get; set; }
        public ICollection<User> Readers { get; set; }
    }
}
