using GraphQL;
using GraphQL.Types;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL.Queries
{
    public class BookQuery : ObjectGraphType
    {
        public BookQuery(IBookService bookService)
        {
            Field<ListGraphType<BookType>>("books")
                .Description("Get all books")
                .Resolve(context =>
                {
                    return bookService.GetAllBooks();
                });

            Field<BookType>("book")
                .Description("Get a book by title")
                .Argument<StringGraphType>("title", "Title of the book")
                .Resolve(context =>
                {
                    var title = context.GetArgument<string>("title");
                    return bookService.GetBookByTitle(title);
                });
        }
    }
}