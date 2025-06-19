using Blazored.LocalStorage;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.PresentationBlazor.Models;
using System.Net.Http.Headers;
using System.Text.Json;

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

        public async Task<ApiResponse<List<AccDto>>> GetAllAsync()
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.GetAsync("api/Acc");
            var content = await response.Content.ReadAsStringAsync();
            var message = response.ReasonPhrase;

            if (response.IsSuccessStatusCode)
            {
                var data = JsonSerializer.Deserialize<List<AccDto>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                           ?? new List<AccDto>();
                return new ApiResponse<List<AccDto>> { IsSuccess = true, Data = data };
            }
            return new ApiResponse<List<AccDto>> { IsSuccess = false, Message = message };
        }

        public async Task<ApiResponse<AccDto>> GetByIdAsync(int id)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.GetAsync($"api/Acc/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var message = response.ReasonPhrase;

            if (response.IsSuccessStatusCode)
            {
                var dto = JsonSerializer.Deserialize<AccDto>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new ApiResponse<AccDto> { IsSuccess = true, Data = dto };
            }
            return new ApiResponse<AccDto> { IsSuccess = false, Message = message };
        }

        public async Task<ApiResponse<bool>> CreateAsync(string name)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PostAsJsonAsync("api/Acc", name);
            var content = await response.Content.ReadAsStringAsync();
            var message = response.ReasonPhrase;

            if (response.IsSuccessStatusCode)
                return new ApiResponse<bool> { IsSuccess = true, Data = true };

            return new ApiResponse<bool> { IsSuccess = false, Message = message };
        }

        public async Task<ApiResponse<bool>> UpdateAsync(int id, string name)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.PutAsJsonAsync($"api/Acc/{id}", name);
            var content = await response.Content.ReadAsStringAsync();
            var message = response.ReasonPhrase;

            if (response.IsSuccessStatusCode)
                return new ApiResponse<bool> { IsSuccess = true, Data = true };

            return new ApiResponse<bool> { IsSuccess = false, Message = message };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var client = await CreateAuthorizedClientAsync();
            var response = await client.DeleteAsync($"api/Acc/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var message = response.ReasonPhrase;

            if (response.IsSuccessStatusCode)
                return new ApiResponse<bool> { IsSuccess = true, Data = true };

            return new ApiResponse<bool> { IsSuccess = false, Message = message };
        }
    }
}
