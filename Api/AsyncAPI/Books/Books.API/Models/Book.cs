using System;
using System.Collections.Generic;

namespace Books.API.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class BookForCreation
    {
        public Guid AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class BookCover
    {
        public string Name { get; set; }
        // public byte[] Content { get; set; }

    }
    public class BookWithCovers : Book
    {
        public IEnumerable<BookCover> BookCovers { get; set; } = new List<BookCover>();

    }
}
