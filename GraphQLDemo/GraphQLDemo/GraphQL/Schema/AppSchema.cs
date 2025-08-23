using GraphQL.Types;
using GraphQLDemo.GraphQL.Mutations;
using GraphQLDemo.GraphQL.Queries;
using GraphQLDemo.GraphQL.Subscriptions;
using GraphQLDemo.Services;

namespace GraphQLDemo.GraphQL.Schema
{
    public class AppSchema : global::GraphQL.Types.Schema
    {
        public AppSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<BookQuery>();
            Mutation = provider.GetRequiredService<BookMutation>();
            Subscription = provider.GetRequiredService<BookSubscription>();
        }
    }
}