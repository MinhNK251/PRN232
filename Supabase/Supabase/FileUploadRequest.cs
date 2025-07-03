using Microsoft.AspNetCore.Mvc;

namespace Supabase
{
    public class FileUploadRequest
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }
}
