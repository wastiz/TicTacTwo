using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebTest.Pages;

public class IndexModel : PageModel
{
    public string Name => (string)TempData[nameof(Name)];

    public void OnGet()
    {
    }

    public IActionResult OnPost([FromBody] Request request)
    {
        Console.WriteLine(request);
        return new JsonResult(new
        {
            username = request.Username,
            password = request.Password
        });
    }

    public class Request
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}