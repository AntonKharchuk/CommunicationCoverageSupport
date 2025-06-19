using Blazored.LocalStorage;

using CommunicationCoverageSupport.Models.DTOs.Auth;
using System.Data;
using CommunicationCoverageSupport.PresentationBlazor.Components.Pages;

using System.Net.Http.Json;

namespace CommunicationCoverageSupport.PresentationBlazor.Services
{
    public class AuthService: IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public AuthService(
            IHttpClientFactory httpClientFactory,
            ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }

        public async Task<AuthResponseDto?> LoginAsync(UserLoginDto dto)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Auth/login", dto);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content
                                       .ReadFromJsonAsync<AuthResponseDto>();

            if (result is not null)
            {
                await _localStorage.SetItemAsync("authToken", result.Token);
            }

            return result;
        }

        public async Task<string?> RegisterUserAsync(UserRegisterDto dto)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Auth/register", dto);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<string?> RegisterAdminAsync(UserRegisterDto dto)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var response = await client.PostAsJsonAsync("api/Auth/register-admin", dto);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }
    }
}
