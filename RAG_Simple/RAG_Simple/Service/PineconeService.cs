using OneOf.Types;
using Pinecone;
using RAG_Simple.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PineconeService
{
    private readonly PineconeClient _pineconeClient;
    private readonly string _indexName;

    public PineconeService(string apiKey, string indexName = "test")
    {
        _pineconeClient = new PineconeClient(apiKey);
        _indexName = indexName;
    }

    public async Task InitializeAsync()
    {
        // Check if index exists, create if not
        var indexes = await _pineconeClient.ListIndexesAsync();
        if (!indexes.Indexes.Any(i => i.Name == _indexName))
        {
            await CreateIndexAsync();
            // Wait for index to be ready
            await Task.Delay(TimeSpan.FromSeconds(30));
        }
    }

    private async Task CreateIndexAsync()
    {
        var createRequest = new CreateIndexRequest
        {
            Name = _indexName,
            Dimension = 1024, // Adjust based on your embedding model
            Metric = MetricType.Cosine,
            Spec = new ServerlessIndexSpec
            {
                Serverless = new ServerlessSpec
                {
                    Cloud = ServerlessSpecCloud.Aws,
                    Region = "us-east-1",
                }
            },
            DeletionProtection = DeletionProtection.Disabled
        };

        await _pineconeClient.CreateIndexAsync(createRequest);
        Console.WriteLine($"Index '{_indexName}' created successfully");
    }

    public async Task<float[]> GenerateEmbeddingAsync(string text, string model = "llama-text-embed-v2")
    {
        try
        {
            var embeddings = await _pineconeClient.Inference.EmbedAsync(new EmbedRequest
            {
                Model = model,
                Inputs = new List<EmbedRequestInputsItem>
                {
                    new() { Text = text }
                },
                Parameters = new Dictionary<string, object?>
                {
                    ["input_type"] = "passage"
                }
            });

            var embeddingData = embeddings.Data.First();
            if (embeddingData.VectorType == VectorType.Dense)
            {
                return embeddingData.AsDense().Values.ToArray();
            }
            else
            {
                throw new Exception("Sparse embeddings not supported");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating embedding: {ex.Message}");
            throw;
        }
    }

    public async Task<string> UpsertDocumentAsync(string text, string documentId = null, Dictionary<string, object> metadata = null)
    {
        try
        {
            // Generate embedding
            var embedding = await GenerateEmbeddingAsync(text);

            // Prepare metadata
            var pineconeMetadata = new Metadata
            {
                ["text"] = text,
                ["created_at"] = DateTime.UtcNow.ToString("o"),
                ["document_id"] = documentId ?? Guid.NewGuid().ToString()
            };

            // Add custom metadata if provided
            if (metadata != null)
            {
                foreach (var kvp in metadata)
                {
                    pineconeMetadata[kvp.Key] = ConvertToMetadataValue(kvp.Value);
                }
            }

            // Create vector
            var vector = new Vector
            {
                Id = Guid.NewGuid().ToString(),
                Values = embedding,
                Metadata = pineconeMetadata
            };

            // Upsert to Pinecone
            var index = _pineconeClient.Index(_indexName);
            var upsertResponse = await index.UpsertAsync(new UpsertRequest
            {
                Vectors = new List<Vector> { vector }
            });

            return vector.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error upserting document: {ex.Message}");
            throw;
        }
    }

    public async Task<List<SearchResult>> SearchSimilarAsync(string query, int topK = 5, string model = "llama-text-embed-v2")
    {
        try
        {
            // Generate embedding for query
            var queryEmbedding = await GenerateEmbeddingAsync(query, model);

            // Search in Pinecone
            var index = _pineconeClient.Index(_indexName);
            var queryResponse = await index.QueryAsync(new QueryRequest
            {
                Vector = queryEmbedding,
                TopK = (uint)topK,
                IncludeMetadata = true,  // Make sure this is true
                IncludeValues = false
            });

            var results = new List<SearchResult>();
            foreach (var match in queryResponse.Matches)
            {
                var result = new SearchResult
                {
                    Id = match.Id,
                    Score = (float)match.Score
                };

                if (match.Metadata != null)
                {
                    if (match.Metadata.TryGetValue("text", out var textObj))
                        result.Text = textObj?.ToString() ?? string.Empty;
                    // Add all metadata
                    foreach (var kvp in match.Metadata)
                        result.Metadata[kvp.Key] = ConvertMetadataValueToSimpleType(kvp.Value);
                }
                results.Add(result);
            }
            return results;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteVectorAsync(string vectorId)
    {
        try
        {
            var index = _pineconeClient.Index(_indexName);
            var deleteResponse = await index.DeleteAsync(new DeleteRequest
            {
                Ids = new[] { vectorId }
            });

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting vector: {ex.Message}");
            return false;
        }
    }

    private MetadataValue? ConvertToMetadataValue(object value)
    {
        return value switch
        {
            string str => new MetadataValue(str),
            int num => new MetadataValue(num),
            long num => new MetadataValue(num),
            float num => new MetadataValue(num),
            double num => new MetadataValue(num),
            bool flag => new MetadataValue(flag),
            DateTime date => new MetadataValue(date.ToString("o")),
            IEnumerable<string> list => new MetadataValue(list.Select(s => new MetadataValue(s)).ToArray()),
            IEnumerable<object> list => new MetadataValue(list.Select(x => ConvertToMetadataValue(x)).ToArray()),
            _ => new MetadataValue(value?.ToString() ?? string.Empty)
        };
    }

    private object ConvertMetadataValueToSimpleType(MetadataValue? value)
    {
        if (value == null) return null;

        // Check which type the MetadataValue contains and convert to simple type
        if (value.IsT0) return value.AsT0; // string
        if (value.IsT1) return value.AsT1; // double
        if (value.IsT2) return value.AsT2; // bool
        if (value.IsT3) // IEnumerable<MetadataValue>
            return value.AsT3?.Select(ConvertMetadataValueToSimpleType).ToArray() ?? Array.Empty<object>();
        if (value.IsT4) // Metadata (dictionary)
        {
            var dict = new Dictionary<string, object>();
            foreach (var kvp in value.AsT4)
            {
                dict[kvp.Key] = ConvertMetadataValueToSimpleType(kvp.Value);
            }
            return dict;
        }

        return value.ToString();
    }
}