using Microsoft.AspNetCore.Mvc;
using RAG_Simple.Models;
using RAG_Simple.Services;

namespace RAG_Simple.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IRAGChatService _ragChatService;
        private readonly IGeminiService _geminiService;

        public ChatController(IRAGChatService ragChatService, IGeminiService geminiService)
        {
            _ragChatService = ragChatService;
            _geminiService = geminiService;
        }

        [HttpPost("rag")]
        public async Task<IActionResult> ChatWithRAG([FromBody] ChatRequest request)
        {
            try
            {
                var response = await _ragChatService.ChatAsync(
                    request.Message,
                    request.SessionId,
                    request.TopK ?? 5
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("direct")]
        public async Task<IActionResult> ChatDirect([FromBody] DirectChatRequest request)
        {
            try
            {
                var response = await _geminiService.ChatAsync(
                    request.Message,
                    request.ConversationHistory
                );

                return Ok(new { Answer = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] GenerateRequest request)
        {
            try
            {
                var response = await _geminiService.GenerateResponseAsync(request.Prompt);
                return Ok(new { Response = response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("history/{sessionId}")]
        public async Task<IActionResult> ClearHistory(string sessionId)
        {
            try
            {
                await _ragChatService.ClearConversationHistoryAsync(sessionId);
                return Ok(new { Message = "Conversation history cleared" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("history/{sessionId}")]
        public IActionResult GetHistory(string sessionId)
        {
            try
            {
                // This would need to be implemented in your service
                // For now, returning not implemented
                return StatusCode(501, new { Message = "Get history not implemented yet" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public string SessionId { get; set; } = "default";
        public int? TopK { get; set; } = 5;
    }

    public class DirectChatRequest
    {
        public string Message { get; set; } = string.Empty;
        public List<ChatMessage>? ConversationHistory { get; set; }
    }

    public class GenerateRequest
    {
        public string Prompt { get; set; } = string.Empty;
    }
}