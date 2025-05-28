using CommunicationCoverageSupport.Models.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;

namespace CommunicationCoverageSupport.Presentation.Pages
{
    public class SimCardsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SimCardsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public List<SimCardDto> SimCards { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.GetAsync("/api/SimCard");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                SimCards = JsonSerializer.Deserialize<List<SimCardDto>>(json, options) ?? new();
            }
        }
    }
}
