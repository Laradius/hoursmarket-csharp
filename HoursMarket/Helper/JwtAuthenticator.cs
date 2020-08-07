using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HoursMarket.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HoursMarket.Helper
{
    public class JwtAuthenticator : IAuthenticator
    {
        private readonly IConfiguration _config;

        public JwtAuthenticator(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAuthenticatorToken(Account account)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Secrets:JwtSecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("ID", account.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Secrets:JwtIssuer"],
                audience: _config["Secrets:JwtAudience"],
                claims,
                expires: DateTime.Now.AddDays(7), signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

    }
}

