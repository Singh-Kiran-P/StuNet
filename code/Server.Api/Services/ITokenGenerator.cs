﻿using System.Threading.Tasks;
using Server.Api.Dtos;
using Server.Api.Models;

namespace VmsApi.Services
{
    public interface ITokenGenerator
    {
        Task<LoginJwtDto> GetTokenAsync(User user);
    }
}