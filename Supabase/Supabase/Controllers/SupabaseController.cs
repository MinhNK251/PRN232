using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Supabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupabaseController : ControllerBase
    {
        private readonly SupabaseService _supabaseService;

        public SupabaseController(SupabaseService supabaseService)
        {
            _supabaseService = supabaseService;
        }

        [HttpPost("file")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                var fileUrl = await _supabaseService.UploadFileAsync(request.File);
                return Ok(new { fileUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error uploading file: {ex.Message}");
            }
        }

        [HttpDelete("file")]
        public async Task<IActionResult> DeleteFile([FromQuery] string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return BadRequest("File URL is required.");

            try
            {
                await _supabaseService.DeleteFileAsync(fileUrl);
                return Ok(new { message = "File deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting file: {ex.Message}");
            }
        }
    }
}
