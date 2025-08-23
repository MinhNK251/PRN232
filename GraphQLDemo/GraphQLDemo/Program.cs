using GraphQL;
using GraphQLDemo.GraphQL.Mutations;
using GraphQLDemo.GraphQL.Queries;
using GraphQLDemo.GraphQL.Schema;
using GraphQLDemo.GraphQL.Subscriptions;
using GraphQLDemo.GraphQL.Types;
using GraphQLDemo.Services;

namespace GraphQLDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddSingleton<IBookService, BookService>();
            builder.Services.AddSingleton<BookType>();
            builder.Services.AddSingleton<BookQuery>();
            builder.Services.AddSingleton<BookMutation>();
            builder.Services.AddSingleton<BookSubscription>();
            builder.Services.AddSingleton<AppSchema>();

            // Add GraphQL
            builder.Services.AddGraphQL(builder =>
            {
                builder.AddSystemTextJson();
                builder.AddSchema<AppSchema>();
                builder.AddGraphTypes();
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseGraphQL<AppSchema>("/graphql");
            // https://localhost:7097/ui/altair
            app.UseGraphQLAltair("/ui/altair");
            app.MapControllers();
            app.Run();
        }
    }
}
