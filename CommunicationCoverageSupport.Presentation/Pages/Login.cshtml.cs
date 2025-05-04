using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CommunicationCoverageSupport.Presentation.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            using var client = new HttpClient();

            var loginPayload = new
            {
                username = Username,
                password = Password
            };

            var json = JsonSerializer.Serialize(loginPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:5062/api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(responseBody);
                    var token = doc.RootElement.GetProperty("token").GetString();

                    // Store JWT in session or cookie
                    HttpContext.Session.SetString("JwtToken", token);

                    return RedirectToPage("/Index");
                }
                else
                {
                    Message = "Login failed: " + await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                Message = "Could not connect to the server.";
            }

            return Page();
        }
    }
}
