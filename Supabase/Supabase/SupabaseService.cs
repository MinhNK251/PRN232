using RestSharp;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Text;

namespace Supabase
{
    public class SupabaseService
    {
        private readonly string _supabaseUrl;
        private readonly string _apiKey;
        private readonly string _bucket;

        public SupabaseService(IConfiguration configuration)
        {
            _supabaseUrl = configuration["Supabase:Url"];
            _apiKey = configuration["Supabase:ApiKey"];
            _bucket = configuration["Supabase:Bucket"];
        }

        public async Task<string?> UploadFileAsync(IFormFile file)
        {
            var safeFileName = SanitizeFileName(file.FileName);
            var filePath = $"{Guid.NewGuid()}_{safeFileName}";
            var uploadUrl = $"{_supabaseUrl}/storage/v1/object/{_bucket}/{filePath}";
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            var client = new RestClient(uploadUrl);
            var request = new RestRequest("", Method.Put);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            request.AddHeader("Content-Type", file.ContentType);
            request.AddParameter("application/octet-stream", fileBytes, ParameterType.RequestBody);
            request.AddBody(fileBytes);
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Upload failed: {response.StatusCode} - {response.Content}");
            }
            return $"{_supabaseUrl}/storage/v1/object/public/{_bucket}/{filePath}";
        }

        public async Task DeleteFileAsync(string fullUrl)
        {
            var prefix = $"{_supabaseUrl}/storage/v1/object/public/{_bucket}/";
            if (!fullUrl.StartsWith(prefix))
                throw new ArgumentException("Invalid file URL provided.");
            var filePath = fullUrl.Substring(prefix.Length);
            var deleteUrl = $"{_supabaseUrl}/storage/v1/object/{_bucket}/{filePath}";
            var client = new RestClient(deleteUrl);
            var request = new RestRequest("", Method.Delete);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");
            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Delete failed: {response.StatusCode} - {response.Content}");
            }
        }

        private static string SanitizeFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            var normalized = fileName.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            fileName = sb.ToString().Normalize(NormalizationForm.FormC);
            fileName = Regex.Replace(fileName, @"[^a-zA-Z0-9_.-]", "_");
            return fileName;
        }
    }
}
