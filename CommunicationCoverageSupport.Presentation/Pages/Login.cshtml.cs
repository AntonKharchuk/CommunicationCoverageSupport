using CommunicationCoverageSupport.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CommunicationCoverageSupport.Presentation.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IHttpClientFactory httpClientFactory, ILogger<LoginModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }


        [BindProperty]
        public LoginRequestDto LoginRequest { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("ApiClient");
            var content = new StringContent(JsonSerializer.Serialize(LoginRequest), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("/api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("JwtToken", token);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMessage = "Invalid username or password.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed.");
                ErrorMessage = "An error occurred while trying to log in.";
                return Page();
            }
        }
    }
}
