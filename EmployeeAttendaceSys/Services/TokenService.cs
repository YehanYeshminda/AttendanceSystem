﻿    using EmployeeAttendaceSys.Entities;
    using EmployeeAttendaceSys.Interfaces;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    namespace EmployeeAttendaceSys.Services
    {
        public class TokenService : ITokenService
        {
            private readonly SymmetricSecurityKey _key;
            public TokenService(IConfiguration config)
            {
                _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            }
        public string CreateToken(User user)
        {
            string passwordHashBase64 = Convert.ToBase64String(user.PasswordHash);
            string passwordSaltBase64 = Convert.ToBase64String(user.PasswordSalt);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString("yyyy-MM-dd")),
                new Claim("PasswordHash", passwordHashBase64),
                new Claim("PasswordSalt", passwordSaltBase64)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false,
                ValidateAudience = false,
                LifetimeValidator = CustomLifetimeValidator
            };

            var principal = tokenHandler.ValidateToken(jwt, validationParameters, out var validatedToken);

            return jwt;
        }

        private bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
            {
                if (expires != null)
                {
                    return expires > DateTime.UtcNow;
                }
                return false;
            }



        }
    }