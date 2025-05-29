using CommunicationCoverageSupport.Models.DTOs.Auth;
using System.Net.Http.Json;

namespace CommunicationCoverageSupport.PresentationBlazor.Services
{
    public class AuthService: IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string?> RegisterAsync(UserRegisterDto dto)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Auth/register", dto);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<AuthResponseDto?> LoginAsync(UserLoginDto dto)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var response = await client.PostAsJsonAsync("api/Auth/login", dto);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<AuthResponseDto>()
                : null;
        }
    }
}
