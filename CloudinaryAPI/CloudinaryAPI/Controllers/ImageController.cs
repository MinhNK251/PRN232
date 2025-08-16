using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudinaryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public ImageController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        // POST: api/image/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(stream, file.FileName);
                    return Ok(new { Url = imageUrl });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/image/delete/{publicId}
        [HttpDelete("delete/{publicId}")]
        public async Task<IActionResult> DeleteImage(string publicId)
        {
            try
            {
                var isDeleted = await _cloudinaryService.DeleteImageAsync(publicId);

                if (isDeleted)
                {
                    return Ok(new { Message = "Image deleted successfully." });
                }
                else
                {
                    return NotFound(new { Message = "Image not found or failed to delete." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
