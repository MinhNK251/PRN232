using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace Client.Pages.Handbag
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public string ModelName { get; set; }
        public string Material { get; set; }
        public List<Models.Handbag> Handbags { get; set; }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var jwt = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwt))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
            var response = await client.GetFromJsonAsync<List<Models.Handbag>>("handbags");
            if (response != null)
            {
                Handbags = response;
            }
            else
            {
                Handbags = new List<Models.Handbag>();
            }
        }

        public async Task OnPostAsync(string modelName, string material)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var jwt = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwt))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }
            var searchUri = $"handbags/search/odata?modelName={modelName}&material={material}";
            var response = await client.GetFromJsonAsync<List<Models.Handbag>>(searchUri);
            if (response != null)
            {
                Handbags = response;                
            }
            else
            {
                Handbags = new List<Models.Handbag>();
            }
            ModelName = modelName;
            Material = material;
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var jwt = Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(jwt))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            }

            var response = await client.DeleteAsync($"handbags/{id}");
            return RedirectToPage("/Handbag/Index");
        }
    }
}
