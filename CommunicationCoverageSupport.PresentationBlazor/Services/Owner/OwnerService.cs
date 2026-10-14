using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Blazored.LocalStorage;

using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.PresentationBlazor.Models;

namespace CommunicationCoverageSupport.PresentationBlazor.Services.Owner
{
    public interface IOwnerService
    {
        Task<ApiResponse<List<OwnerDto>>> GetAllAsync();
        Task<ApiResponse<OwnerDto>> GetByIdAsync(long id);
        Task<ApiResponse<bool>> CreateAsync(OwnerDto dto);
        Task<ApiResponse<bool>> UpdateAsync(OwnerDto dto);
        Task<ApiResponse<bool>> DeleteAsync(long id);
    }

    public class OwnerService : IOwnerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public OwnerService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }

        private async Task<HttpClient> CreateAuthorizedClientAsync()
        {
            var client = _httpClientFactory.CreateClient("ApiClient");
            var token = await _localStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<ApiResponse<List<OwnerDto>>> GetAllAsync()
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync("api/Owner");
            var msg = resp.ReasonPhrase;

            if (resp.IsSuccessStatusCode)
            {
                var list = await resp.Content.ReadFromJsonAsync<List<OwnerDto>>()
                           ?? new List<OwnerDto>();
                return new ApiResponse<List<OwnerDto>> { IsSuccess = true, Data = list };
            }
            return new ApiResponse<List<OwnerDto>> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<OwnerDto>> GetByIdAsync(long id)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync($"api/Owner/{id}");
            var msg = resp.ReasonPhrase;

            if (resp.IsSuccessStatusCode)
            {
                var dto = await resp.Content.ReadFromJsonAsync<OwnerDto>();
                return new ApiResponse<OwnerDto> { IsSuccess = true, Data = dto! };
            }
            return new ApiResponse<OwnerDto> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> CreateAsync(OwnerDto dto)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.PostAsJsonAsync("api/Owner", dto);
            var msg = resp.ReasonPhrase;

            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OwnerDto dto)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.PutAsJsonAsync("api/Owner", dto);
            var msg = resp.ReasonPhrase;

            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(long id)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.DeleteAsync($"api/Owner/{id}");
            var msg = resp.ReasonPhrase;

            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }
    }
}
