using GraphQL.Types;
using GraphQLDemo.Models;

namespace GraphQLDemo.GraphQL.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(x => x.Title).Description("The title of the book");
            Field(x => x.Author).Description("The author of the book");
            Field(x => x.PublishedYear).Description("The year the book was published");
        }
    }
}
