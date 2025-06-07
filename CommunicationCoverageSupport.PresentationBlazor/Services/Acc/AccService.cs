using Blazored.LocalStorage;
using CommunicationCoverageSupport.Models.DTOs;
using System.Net.Http.Headers;

namespace CommunicationCoverageSupport.PresentationBlazor.Services.Acc
{
    public class AccService : IAccService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public AccService(
            IHttpClientFactory httpClientFactory,
            ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }

        private async Task<HttpClient> CreateAuthorizedClientAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<List<AccDto>> GetAllAsync()
        {
            var client = await CreateAuthorizedClientAsync();
            return await client
                .GetFromJsonAsync<List<AccDto>>("api/Acc")
                   ?? new List<AccDto>();
        }

        public async Task<AccDto?> GetByIdAsync(int id)
        {
            var client = await CreateAuthorizedClientAsync();
            return await client
                .GetFromJsonAsync<AccDto>($"api/Acc/{id}");
        }

        public async Task CreateAsync(string name)
        {
            var client = await CreateAuthorizedClientAsync();
            await client.PostAsJsonAsync("api/Acc", name);
        }

        public async Task UpdateAsync(int id, string name)
        {
            var client = await CreateAuthorizedClientAsync();
            await client.PutAsJsonAsync($"api/Acc/{id}", name);
        }

        public async Task DeleteAsync(int id)
        {
            var client = await CreateAuthorizedClientAsync();
            await client.DeleteAsync($"api/Acc/{id}");
        }
    }
}
