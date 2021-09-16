using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Dii_TheaterManagement_Bff.Features.SyntheticBehavior
{
    public class UserInfoAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserInfoAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public UserInfoUserType UserType
        {
            get {
                var request = _httpContextAccessor.HttpContext.Request;
                if (!request.Headers.ContainsKey("Authorization")) throw new InvalidOperationException("Authorization header is required");
                string authorizationHeaderValue = request.Headers["Authorization"];

                var parts = authorizationHeaderValue.Split(' ');
                if (parts[0] != "Bearer") throw new InvalidOperationException($"\"Bearer\" token type is required; found \"{parts[0]}\"");

                if (parts.Length != 2) throw new InvalidOperationException($"\"Bearer\" token type must contain just 1 encrypted JWT; found; found {parts.Length - 1}");
                string jwt = parts[1];

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwt);
                var userTypeClaim = jwtSecurityToken.Claims.SingleOrDefault(c => c.Type == "x-usertype");
                if (userTypeClaim == null) throw new InvalidOperationException("x-usertype Claim is required.");
                switch (userTypeClaim.Value)
                {
                    case "synthetic":
                        return UserInfoUserType.Synthetic;
                    case "real":
                        return UserInfoUserType.Real;
                    default:
                        throw new InvalidOperationException($"x-usertype Claim must be \"synthetic\" or \"real\" but was \"{userTypeClaim.Value}\".");
                }
            }
        }

        public string TheaterCode
        {
            get
            {
                var request = _httpContextAccessor.HttpContext.Request;

                if (!request.Headers.ContainsKey("Authorization")) throw new InvalidOperationException("Authorization header is required");

                string authorizationHeaderValue = request.Headers["Authorization"];

                var parts = authorizationHeaderValue.Split(' ');
                if (parts[0] != "Bearer") throw new InvalidOperationException($"\"Bearer\" token type is required; found \"{parts[0]}\"");

                if (parts.Length != 2) throw new InvalidOperationException($"\"Bearer\" token type must contain just 1 encrypted JWT; found; found {parts.Length - 1}");
                string jwt = parts[1];

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwt);
                var theaterCodeClaim = jwtSecurityToken.Claims.SingleOrDefault(c => c.Type == "x-theatercode");
                if (theaterCodeClaim == null) throw new InvalidOperationException("x-theatercode Claim is required.");
                return theaterCodeClaim.Value;
            }
        }
    }
}
