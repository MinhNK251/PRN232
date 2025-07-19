using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            Response.Cookies.Delete("jwt");
            HttpContext.Session.Remove("Role");
            return RedirectToPage("/Login");
        }
    }
}
