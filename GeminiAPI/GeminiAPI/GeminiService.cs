using Newtonsoft.Json;
using System.Text;

namespace GeminiAPI
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _geminiApiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _geminiApiKey = configuration["GeminiApiKey"];
        }

        public async Task<string> GetDailyQuotesAsync()
        {
            var prompt = @"
            Please provide three inspirational quotes for meditation, one for each part of the day: morning, noon, and evening.
            Return the response in JSON format with the following structures:
            {
                'morningQuote': 'Your morning quote here',
                'noonQuote': 'Your afternoon quote here',
                'eveningQuote': 'Your evening quote here'
            }
            return the json only without using keyword json.";

            var jsonRequest = new
            {
                contents = new[] // We're still using contents array for the request
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(jsonRequest), Encoding.UTF8, "application/json");

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent")
            {
                Content = requestContent
            };

            // Add the API key as a header
            requestMessage.Headers.Add("X-goog-api-key", _geminiApiKey);

            var response = await _httpClient.SendAsync(requestMessage);

            // Read the response body as a string and log it for debugging
            var responseBodySuccess = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response from Gemini API: {responseBodySuccess}");

            // Deserialize the response safely
            var result = JsonConvert.DeserializeObject<dynamic>(responseBodySuccess);

            // Check if the result is valid before accessing it
            if (result?.candidates == null || result?.candidates.Count == 0 || result?.candidates[0]?.content?.parts == null || result?.candidates[0]?.content?.parts.Count == 0)
            {
                throw new Exception("Invalid response structure from Gemini API.");
            }

            // Safely access the text field
            var responseText = result?.candidates[0]?.content?.parts[0]?.text?.ToString();

            if (string.IsNullOrEmpty(responseText))
            {
                throw new Exception("The response text from Gemini API is empty or null.");
            }

            return responseText;
        }
    }
}