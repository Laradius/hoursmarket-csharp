using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using HoursMarket.Dto;
using HoursMarket.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HoursMarket.Helper
{
    public class JwtAuthenticator : IAuthenticator
    {




        public string GenerateRegistrationToken(AccountRegistrationDto accountRegistration)
        {


            SymmetricSecurityKey securityKey = null;
            SigningCredentials credentials = null;

#if DEBUG
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["Secrets:JwtSecretKey"]));


#elif RELEASE
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["Credentials:JwtSecretKey"]));
#endif


            
            credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);




            var claims = new[]
            {
                new Claim("e-mail", accountRegistration.Email),
                new Claim("name", accountRegistration.Name)
            };

            var token = new JwtSecurityToken(
#if DEBUG
                issuer: Startup.StaticConfig["Secrets:JwtIssuer"],
                audience: Startup.StaticConfig["Secrets:JwtAudience"],

#elif RELEASE
                   issuer: Startup.StaticConfig["Credentials:JwtIssuer"],
                audience: Startup.StaticConfig["Credentials:JwtAudience"],
#endif
                claims,
                expires: DateTime.Now.AddDays(7), signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        public string GenerateAccountAccessToken(Account account)
        {


            SymmetricSecurityKey securityKey = null;
            SigningCredentials credentials = null;

#if DEBUG
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["Secrets:JwtSecretKey"]));


#elif RELEASE
            securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["Credentials:JwtSecretKey"]));
#endif

            credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("ID", account.Id.ToString())
            };

            var token = new JwtSecurityToken(
#if DEBUG
                issuer:  Startup.StaticConfig["Secrets:JwtIssuer"],
                audience:  Startup.StaticConfig["Secrets:JwtAudience"],

#elif RELEASE
                   issuer: Startup.StaticConfig["Credentials:JwtIssuer"],
                audience: Startup.StaticConfig["Credentials:JwtAudience"],
#endif
                claims,
                expires: DateTime.Now.AddDays(7), signingCredentials: credentials);

            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        public bool ValidateToken(string authToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            return true;
        }


        public static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
#if DEBUG
                ValidIssuer = Startup.StaticConfig["Secrets:JwtIssuer"],
                ValidAudience = Startup.StaticConfig["Secrets:JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["Secrets:JwtSecretKey"]))


#elif RELEASE
                ValidIssuer = Startup.StaticConfig["Credentials:JwtIssuer"],
                ValidAudience = Startup.StaticConfig["Credentials:JwtAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.StaticConfig["Credentials:JwtSecretKey"]))
#endif


            };
        }

    }
}

