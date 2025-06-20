using AiChatApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AiChatApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ChatController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ChatRequest request)
    {
        var apiKey = _configuration["OpenRouter:ApiKey"];
        if (string.IsNullOrEmpty(apiKey))
            return BadRequest("API Key not configured.");

        var client = _httpClientFactory.CreateClient();

        var apiUrl = "https://openrouter.ai/api/v1/chat/completions";

        var body = new
        {
            model = "deepseek/deepseek-chat-v3-0324:free",  // You can pick any free model from OpenRouter
            messages = new[]
            {
                new { role = "user", content = request.Message }
            }
        };

        var json = JsonSerializer.Serialize(body);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiUrl);
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.SendAsync(httpRequest);
        var responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, $"OpenRouter API error: {response.StatusCode}, Response: {responseString}");
        }

        using var doc = JsonDocument.Parse(responseString);
        var reply = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return Ok(new ChatResponse { Reply = reply });
    }
}
