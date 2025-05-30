﻿using CommunicationCoverageSupport.Models.DTOs.Auth;

namespace CommunicationCoverageSupport.BLL.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
    }
}
