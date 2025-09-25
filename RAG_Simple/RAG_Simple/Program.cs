
using Microsoft.Extensions.Options;
using RAG_Simple.Config;
using RAG_Simple.Service;
using RAG_Simple.Services;

namespace RAG_Simple
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add HttpClient for Gemini
            builder.Services.AddHttpClient<IGeminiService, GeminiService>();

            // Add services
            builder.Services.AddScoped<IGeminiService, GeminiService>();
            builder.Services.AddScoped<IRAGChatService, RAGChatService>();

            // Configure Pinecone settings from appsettings.json
            builder.Services.Configure<PineconeConfig>(
                builder.Configuration.GetSection("Pinecone"));

            // Register Pinecone services
            builder.Services.AddSingleton<PineconeService>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<PineconeConfig>>().Value;
                var service = new PineconeService(config.ApiKey, config.IndexName);
                // Initialize async in background
                _ = service.InitializeAsync();
                return service;
            });

            builder.Services.AddSingleton<HandbookPineconeService>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<PineconeConfig>>().Value;
                return new HandbookPineconeService(config.ApiKey, config.IndexName);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
