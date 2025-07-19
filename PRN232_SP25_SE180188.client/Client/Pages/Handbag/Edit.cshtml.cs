using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Pages.Handbag
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Models.Handbag Handbag { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var jwt = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwt))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }

            var response = await client.GetFromJsonAsync<Models.Handbag>($"handbags/{id}");

            if (response != null)
            {
                Handbag = response;
            }
            else
            {
                return RedirectToPage("/Handbag/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var jwt = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwt))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }

            var content = new StringContent(JsonSerializer.Serialize(Handbag), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"handbags/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Handbag/Index");  // Redirect to index or success page
            }
            else
            {
                // Handle failure
                return Page();
            }
        }
    }
}
