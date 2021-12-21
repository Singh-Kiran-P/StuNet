﻿// @Kiran

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Server.Api.Dtos;
using Server.Api.Models;

namespace Server.Api.Services {
    [ExcludeFromCodeCoverage]
    public class JwtTokenManager : ITokenManager {
        private readonly UserManager<User> _userManager;
        private readonly IConfigurationSection _jwtSettings;

        public JwtTokenManager (UserManager<User> userManager, IConfiguration jwtSettings) {
            _userManager = userManager;
            _jwtSettings = jwtSettings.GetSection ("JwtSettings");
        }

        public async Task<LoginJwtDto> GetTokenAsync (User user) {
            var signingCredentials = GetSigningCredentials ();
            var claims = GetClaims (user);
            var tokenOptions = GenerateTokenOptions (signingCredentials, await claims);
            var token = new JwtSecurityTokenHandler ().WriteToken (tokenOptions);
            return new LoginJwtDto (token);
        }

        private SigningCredentials GetSigningCredentials () {
            var key = Encoding.UTF8.GetBytes (_jwtSettings.GetSection ("secretKey").Value);
            var secret = new SymmetricSecurityKey (key);

            return new SigningCredentials (secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GenerateTokenOptions (SigningCredentials signingCredentials, List<Claim> claims) {
            var tokenOptions = new JwtSecurityToken (
                issuer: _jwtSettings.GetSection ("validIssuer").Value,
                audience: _jwtSettings.GetSection ("validAudience").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes (Convert.ToDouble (_jwtSettings.GetSection ("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        private async Task<List<Claim>> GetClaims (User user) {
            var claims = new List<Claim> {
                new Claim ("username", user.Email),
                new Claim ("userref", user.Id)
            };
            var roles = await _userManager.GetRolesAsync (user);
            System.Diagnostics.Debug.Write ($"{user.Email} has roles:");
            foreach (var role in roles) {
                System.Diagnostics.Debug.Write ($"[{role}]");
            }

            foreach (var role in roles) {
                claims.Add (new Claim ("roles", role));
            }
            return claims;
        }

        public bool ValidateToken (string token) {
            try {
                var key = Encoding.UTF8.GetBytes (_jwtSettings.GetSection ("secretKey").Value);
                var secret = new SymmetricSecurityKey (key);

                var tokenHandler = new JwtSecurityTokenHandler ();
                tokenHandler.ValidateToken (token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey (key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                return true;
            } catch {
                return false;
            }
        }
    }
}
