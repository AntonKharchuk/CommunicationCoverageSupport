﻿using CommunicationCoverageSupport.Models.DTOs;

using System.Threading.Tasks;

namespace CommunicationCoverageSupport.BLL.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequestDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginDto);
    }
}
