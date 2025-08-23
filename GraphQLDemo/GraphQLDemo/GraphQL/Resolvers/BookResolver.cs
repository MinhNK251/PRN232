using GraphQLDemo.Models;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL.Resolvers
{
    public class BookResolver
    {
        private readonly IBookService _bookService;

        public BookResolver(IBookService bookService)
        {
            _bookService = bookService;
        }

        public IEnumerable<Book> ResolveBooks()
        {
            return _bookService.GetAllBooks();
        }

        public Book ResolveBook(string title)
        {
            return _bookService.GetBookByTitle(title);
        }

        public Book ResolveAddBook(Book book)
        {
            return _bookService.AddBook(book);
        }
    }
}
