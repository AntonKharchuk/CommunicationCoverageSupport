using Blazored.LocalStorage;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;
using CommunicationCoverageSupport.PresentationBlazor.Models;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CommunicationCoverageSupport.PresentationBlazor.Services.SimCard
{
    public class SimCardService : ISimCardService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public SimCardService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
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

        private string? ExtractMessage(HttpResponseMessage response)
        {
            var result = response.ReasonPhrase;
            
            if (response.Content.Headers.ContentType?.MediaType == "text/plain")
            {
                var content = response.Content.ReadAsStringAsync().Result;
                result += $" - \n{content}";
            }
            return result;
        }

        public async Task<ApiResponse<List<SimCardDto>>> GetAllAsync()
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync("api/SimCard");
            var msg = ExtractMessage(resp);
            if (resp.IsSuccessStatusCode)
            {
                var data = await resp.Content.ReadFromJsonAsync<List<SimCardDto>>() ?? new List<SimCardDto>();
                var limitedData = data.Take(10).ToList();
                return new ApiResponse<List<SimCardDto>> { IsSuccess = true, Data = limitedData };
            }
            return new ApiResponse<List<SimCardDto>> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<SimCardDto>> GetByIccidAsync(string iccid)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync($"api/SimCard/{iccid}");
            var msg = ExtractMessage(resp);
            if (resp.IsSuccessStatusCode)
            {
                var dto = await resp.Content.ReadFromJsonAsync<SimCardDto>();
                return new ApiResponse<SimCardDto> { IsSuccess = true, Data = dto, };
            }
            return new ApiResponse<SimCardDto> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<SimCardFullInfoDto>> GetFullByIccidAsync(string iccid)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync($"api/SimCard/full/{iccid}");
            var msg = ExtractMessage(resp);
            if (resp.IsSuccessStatusCode)
            {
                var info = await resp.Content.ReadFromJsonAsync<SimCardFullInfoDto>();
                return new ApiResponse<SimCardFullInfoDto> { IsSuccess = true, Data = info };
            }
            return new ApiResponse<SimCardFullInfoDto> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> CreateAsync(SimCardDto dto)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.PostAsJsonAsync("api/SimCard", dto);
            var msg = ExtractMessage(resp);
            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> UpdateAsync(SimCardDto dto)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.PutAsJsonAsync("api/SimCard", dto);
            var msg = ExtractMessage(resp);
            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> UpdateInstalledAsync(UpdateInstalledRequestDto request)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.PatchAsJsonAsync("api/SimCard/installed", request);
            var msg = ExtractMessage(resp);
            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true, Message = msg }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> DrainAsync(SimCardPrimaryKeyDto request)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.PostAsJsonAsync("api/SimCard/drain", request);
            var msg = ExtractMessage(resp);
            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<List<SimCardDrainDto>>> GetDrainsAsync()
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync("api/SimCardDrain");
            var msg = ExtractMessage(resp);
            if (resp.IsSuccessStatusCode)
            {
                var list = await resp.Content.ReadFromJsonAsync<List<SimCardDrainDto>>() ?? new List<SimCardDrainDto>();
                return new ApiResponse<List<SimCardDrainDto>> { IsSuccess = true, Data = list };
            }
            return new ApiResponse<List<SimCardDrainDto>> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<SimCardDrainDto>> GetDrainByIccidAsync(string iccid)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync($"api/SimCardDrain/{iccid}");
            var msg = ExtractMessage(resp);
            if (resp.IsSuccessStatusCode)
            {
                var dto = await resp.Content.ReadFromJsonAsync<SimCardDrainDto>();
                return new ApiResponse<SimCardDrainDto> { IsSuccess = true, Data = dto };
            }
            return new ApiResponse<SimCardDrainDto> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<SimCardDrainFullInfoDto>> GetDrainFullByIccidAsync(string iccid)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.GetAsync($"api/SimCardDrain/full/{iccid}");
            var msg = ExtractMessage(resp);
            if (resp.IsSuccessStatusCode)
            {
                var info = await resp.Content.ReadFromJsonAsync<SimCardDrainFullInfoDto>();
                return new ApiResponse<SimCardDrainFullInfoDto> { IsSuccess = true, Data = info };
            }
            return new ApiResponse<SimCardDrainFullInfoDto> { IsSuccess = false, Message = msg };
        }

        public async Task<ApiResponse<bool>> DeleteDrainAsync(SimCardPrimaryKeyDto request)
        {
            var client = await CreateAuthorizedClientAsync();
            var resp = await client.SendAsync(
                new HttpRequestMessage(HttpMethod.Delete, "api/SimCardDrain") { Content = JsonContent.Create(request) });
            var msg = ExtractMessage(resp);
            return resp.IsSuccessStatusCode
                ? new ApiResponse<bool> { IsSuccess = true, Data = true }
                : new ApiResponse<bool> { IsSuccess = false, Message = msg };
        }
    }
}
