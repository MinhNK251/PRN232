using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeminiAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {
        private readonly GeminiService _geminiService;

        public GeminiController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        // Endpoint to get daily quotes
        [HttpGet("daily-quotes")]
        public async Task<IActionResult> GetDailyQuotes()
        {
            try
            {
                var quotes = await _geminiService.GetDailyQuotesAsync();
                return Ok(new { Quotes = quotes });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
