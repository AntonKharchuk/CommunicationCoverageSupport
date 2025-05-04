using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.Presentation.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string CompanyName { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            using var client = new HttpClient();
            var registerPayload = new
            {
                username = Username,
                password = Password,
                companyName = CompanyName
            };

            var json = JsonSerializer.Serialize(registerPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("http://localhost:5062/api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    Message = "Registration successful! You can now log in.";
                }
                else
                {
                    Message = "Registration failed: " + await response.Content.ReadAsStringAsync();
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
