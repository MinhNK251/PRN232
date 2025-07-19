using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

namespace Client.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public LoginModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();
        public string ErrorMessage { get; set; } = string.Empty;

        public class InputModel
        {
            [Required]
            public string Email { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            var client = _clientFactory.CreateClient();
            var apiUrl = "https://localhost:7062/api/auth";
            var options = new JsonSerializerOptions { PropertyNamingPolicy = null };
            var json = JsonSerializer.Serialize(Input, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(apiUrl, content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var doc = JsonDocument.Parse(responseBody);
                var token = doc.RootElement.GetProperty("token").GetString();
                var role = doc.RootElement.GetProperty("role").GetString(); 
                Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = false, Secure = true });
                HttpContext.Session.SetString("Role", role);
                return RedirectToPage("/Handbag/Index");
            }
            else
            {
                ErrorMessage = "Invalid Email or Password!";
                return Page();
            }
        }
    }
}
