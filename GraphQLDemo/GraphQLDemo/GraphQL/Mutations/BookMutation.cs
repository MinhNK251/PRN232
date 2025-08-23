using GraphQL;
using GraphQL.Types;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Models;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL.Mutations
{
    public class BookMutation : ObjectGraphType
    {
        public BookMutation(IBookService bookService)
        {
            Field<BookType>("addBook")
                .Description("Add a new book")
                .Argument<NonNullGraphType<BookInputType>>("book", "The book to add")
                .Resolve(context =>
                {
                    var book = context.GetArgument<Book>("book");
                    return bookService.AddBook(book);
                });

            Field<BookType>("updateBook")
                .Description("Update an existing book")
                .Argument<NonNullGraphType<StringGraphType>>("oldTitle", "Current title of the book to update")
                .Argument<NonNullGraphType<BookInputType>>("updatedBook", "The updated book data")
                .Resolve(context =>
                {
                    var oldTitle = context.GetArgument<string>("oldTitle");
                    var updatedBook = context.GetArgument<Book>("updatedBook");

                    var result = bookService.UpdateBook(oldTitle, updatedBook);

                    if (result == null)
                        throw new ExecutionError($"Book with title '{oldTitle}' not found.");

                    return result;
                });

            Field<BooleanGraphType>("deleteBook")
                .Description("Delete a book by title")
                .Argument<NonNullGraphType<StringGraphType>>("title", "Title of the book to delete")
                .Resolve(context =>
                {
                    var title = context.GetArgument<string>("title");
                    return bookService.DeleteBook(title);
                });
        }
    }

    public class BookInputType : InputObjectGraphType<Book>
    {
        public BookInputType()
        {
            Field(x => x.Title).Description("The title of the book");
            Field(x => x.Author).Description("The author of the book");
            Field(x => x.PublishedYear).Description("The year the book was published");
        }
    }
}