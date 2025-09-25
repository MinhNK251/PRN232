using Newtonsoft.Json;
using RAG_Simple.Models;
using System.Text;

namespace RAG_Simple.Services
{
    public interface IGeminiService
    {
        Task<string> GenerateResponseAsync(string prompt);
        Task<string> GenerateResponseAsync(string prompt, List<string> context);
        Task<string> ChatAsync(string message, List<ChatMessage> conversationHistory = null);
    }

    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;
        private readonly string _model;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _geminiApiKey = configuration["Gemini:ApiKey"];
            _model = configuration["Gemini:Model"] ?? "gemini-2.0-flash";
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            return await GenerateContentAsync(prompt);
        }

        public async Task<string> GenerateResponseAsync(string prompt, List<string> context)
        {
            var contextPrompt = BuildContextPrompt(context, prompt);
            return await GenerateContentAsync(contextPrompt);
        }

        public async Task<string> ChatAsync(string message, List<ChatMessage> conversationHistory = null)
        {
            var messages = new List<object>();

            // Add system prompt
            messages.Add(new
            {
                role = "user",
                parts = new[]
                {
                    new { text = "You are a helpful assistant that provides accurate and concise answers based on the context provided." }
                }
            });

            // Add conversation history if available
            if (conversationHistory != null)
            {
                foreach (var chatMessage in conversationHistory)
                {
                    messages.Add(new
                    {
                        role = chatMessage.Role.ToString().ToLower(),
                        parts = new[] { new { text = chatMessage.Content } }
                    });
                }
            }

            // Add current message
            messages.Add(new
            {
                role = "user",
                parts = new[] { new { text = message } }
            });

            var requestData = new
            {
                contents = messages,
                generationConfig = new
                {
                    temperature = 0.7,
                    topK = 40,
                    topP = 0.95,
                    maxOutputTokens = 1024
                }
            };

            return await SendGeminiRequestAsync(requestData);
        }

        private async Task<string> GenerateContentAsync(string prompt)
        {
            var requestData = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.3, // Lower temperature for more factual responses
                    topK = 40,
                    topP = 0.8,
                    maxOutputTokens = 1024
                }
            };

            return await SendGeminiRequestAsync(requestData);
        }

        private string BuildContextPrompt(List<string> context, string question)
        {
            var prompt = new StringBuilder();

            prompt.AppendLine("Please answer the following question based on the provided context information.");
            prompt.AppendLine("If the context doesn't contain the answer, say 'I don't have enough information to answer that question.'");
            prompt.AppendLine();
            prompt.AppendLine("Context information:");
            prompt.AppendLine("====================");

            foreach (var contextText in context)
            {
                prompt.AppendLine(contextText);
                prompt.AppendLine("---");
            }

            prompt.AppendLine("====================");
            prompt.AppendLine();
            prompt.AppendLine($"Question: {question}");
            prompt.AppendLine("Answer:");

            return prompt.ToString();
        }

        private async Task<string> SendGeminiRequestAsync(object requestData)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_geminiApiKey}";

            var jsonRequest = JsonConvert.SerializeObject(requestData);
            var requestContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API error: {response.StatusCode}, {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseBody);

            // Extract the response text
            if (result?.candidates != null && result.candidates.Count > 0 &&
                result.candidates[0]?.content?.parts != null && result.candidates[0].content.parts.Count > 0)
            {
                return result.candidates[0].content.parts[0].text.ToString();
            }

            throw new Exception("Invalid response structure from Gemini API.");
        }
    }
}