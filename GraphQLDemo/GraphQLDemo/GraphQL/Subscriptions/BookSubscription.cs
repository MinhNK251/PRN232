using GraphQL.Types;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Models;
using GraphQLDemo.Services;
using System.Reactive.Linq;

namespace GraphQLDemo.GraphQL.Subscriptions
{
    public class BookSubscription : ObjectGraphType
    {
        public BookSubscription(IBookService bookService)
        {
            Field<BookType>("bookAdded")
                .ResolveStream(context =>
                {
                    return Observable.FromEvent<EventHandler<Book>, Book>(
                        conversion: handler => (sender, book) => handler(book),
                        addHandler: handler => bookService.BookAdded += handler,
                        removeHandler: handler => bookService.BookAdded -= handler
                    );
                });

            Field<BookType>("bookUpdated")
                .ResolveStream(context =>
                {
                    return Observable.FromEvent<EventHandler<Book>, Book>(
                        conversion: handler => (sender, book) => handler(book),
                        addHandler: handler => bookService.BookUpdated += handler,
                        removeHandler: handler => bookService.BookUpdated -= handler
                    );
                });

            Field<StringGraphType>("bookDeleted")
                .ResolveStream(context =>
                {
                    return Observable.FromEvent<EventHandler<string>, string>(
                        conversion: handler => (sender, title) => handler(title),
                        addHandler: handler => bookService.BookDeleted += handler,
                        removeHandler: handler => bookService.BookDeleted -= handler
                    );
                });
        }
    }
}