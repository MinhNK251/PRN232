using GraphQLDemo.Models;

namespace GraphQLDemo.Services
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetBookByTitle(string title);
        Book AddBook(Book book);
        Book UpdateBook(string oldTitle, Book updatedBook);
        bool DeleteBook(string title);
        event EventHandler<Book> BookAdded;
        event EventHandler<Book> BookUpdated;
        event EventHandler<string> BookDeleted;
    }

    public class BookService : IBookService
    {
        private readonly List<Book> _books = new()
        {
            new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", PublishedYear = 1925 },
            new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", PublishedYear = 1960 },
            new Book { Title = "1984", Author = "George Orwell", PublishedYear = 1949 }
        };

        public event EventHandler<Book> BookAdded;
        public event EventHandler<Book> BookUpdated;
        public event EventHandler<string> BookDeleted;

        public List<Book> GetAllBooks()
        {
            return _books;
        }

        public Book GetBookByTitle(string title)
        {
            return _books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public Book AddBook(Book book)
        {
            _books.Add(book);
            BookAdded?.Invoke(this, book);
            return book;
        }

        public Book UpdateBook(string oldTitle, Book updatedBook)
        {
            var existingBook = GetBookByTitle(oldTitle);
            if (existingBook == null)
                return null;

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.PublishedYear = updatedBook.PublishedYear;

            BookUpdated?.Invoke(this, existingBook);
            return existingBook;
        }

        public bool DeleteBook(string title)
        {
            var book = GetBookByTitle(title);
            if (book == null)
                return false;

            _books.Remove(book);
            BookDeleted?.Invoke(this, title);
            return true;
        }
    }
}
