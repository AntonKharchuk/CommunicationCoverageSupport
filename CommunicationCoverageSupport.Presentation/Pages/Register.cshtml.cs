using CommunicationCoverageSupport.Models.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CommunicationCoverageSupport.Presentation.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(IHttpClientFactory httpClientFactory, ILogger<RegisterModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [BindProperty]
        public UserRegisterDto RegisterRequest { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var client = _httpClientFactory.CreateClient("ApiClient");
            var content = new StringContent(JsonSerializer.Serialize(RegisterRequest), Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("/api/Auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Login");
                }
                else
                {
                    ErrorMessage = "Registration failed. Please try again.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed.");
                ErrorMessage = "An error occurred while trying to register.";
                return Page();
            }
        }
    }
}
