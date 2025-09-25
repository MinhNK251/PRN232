using Pinecone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PineConeTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var pinecone = new PineconeClient("your-pinecone-key");
            try
            {
                // Create embedding with required input_type parameter
                var embeddings = await pinecone.Inference.EmbedAsync(new EmbedRequest
                {
                    Model = "llama-text-embed-v2",
                    Inputs = new List<EmbedRequestInputsItem>
                    {
                        new() { Text = "The cat sits on the mat." }
                    },
                    Parameters = new Dictionary<string, object?>
                    {
                        ["input_type"] = "passage"
                    }
                });

                // Use the embedding
                var embedding = embeddings.Data.First().AsDense();

                // Convert to array first, then use Take()
                var embeddingArray = embedding.Values.ToArray();
                Console.WriteLine($"Embedding dimension: {embeddingArray.Length}");
                Console.WriteLine($"First 10 values: {string.Join(", ", embeddingArray.Take(10))}");
                Console.WriteLine($"All values: {string.Join(", ", embeddingArray)}");
            }
            catch (PineconeException ex)
            {
                Console.WriteLine($"Pinecone Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }
        }
    }
}