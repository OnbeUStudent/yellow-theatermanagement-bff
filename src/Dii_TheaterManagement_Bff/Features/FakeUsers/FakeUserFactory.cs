using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Dii_TheaterManagement_Bff.Features.FakeUsers
{
    public class FakeUserFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FakeUserFactory(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<FakeUser> GetAllUsers()
        {
            var users = _configuration.GetSection("FakeUsers").Get<FakeUser[]>();
            foreach (var user in users)
            {
                user.BearerToken = GetBearerToken(user, "MyBackend", "MyBackend", DateTime.Now.AddDays(365 * 1000));
                yield return user;
            }
        }

        public FakeUser GetUser(string sub)
        {
            var allUsers = GetAllUsers();
            var user = allUsers.SingleOrDefault(u => u.Sub == sub);
            if (user != null)
            {
                user.BearerToken = GetBearerToken(user, "MyBackend", "MyBackend", DateTime.Now.AddDays(365 * 1000));
            }
            return user;
        }

        public static string GetBearerToken(FakeUser user, string issuer, string audience, DateTime expiresUtc)
        {
            List<Claim> claims = new()
            {
                new Claim("sub", user.Sub),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("x-usertype", user.IsSynthetic ? "synthetic" : "real"),
                new Claim("x-theatercode", user.TheaterCode),
            };

            var handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = audience,
                Issuer = issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = expiresUtc,
            };
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}