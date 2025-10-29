using Domain;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Infraestructure
{
    public class AuthService(
        IConfiguration configuration,
        UserManager<UserExtension> userManager,
        IHttpContextAccessor httpContextAccessor,
        IRefreshTokenStore token,
        IVaultSecretsProvider vaultSecretsProvider)
        : IAuthService
    {
        public async Task<(string AccessToken, string RefreshToken)> GenerateTokenAsync(User userApp)
        {
            var secrets = vaultSecretsProvider.GetSecretsAsync().GetAwaiter().GetResult();
            var secKey = secrets["JwtSecretKey"];
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secKey));
            var user = userApp.Adapt<UserExtension>();
            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.UserName!),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenOptions = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"])),
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = GenerateSecureToken();
            var expiration = TimeSpan.FromHours(Convert.ToDouble(jwtSettings["RefreshTokenExpirationHours"]));

            //Se guardan en el almacén de RefreshTokens (Redis)
            await token.SaveTokenAsync(user.Id, refreshToken, expiration);

            //Agregamos a loas Cookies el AccessToken y RefreshToken
            SetAuthCookies(accessToken, refreshToken);


            return (accessToken, refreshToken);
        }


        public async Task<(string AccessToken, string RefreshToken, User? User)> RefreshTokensAsync()
        {
            var context = httpContextAccessor.HttpContext!;

            var refreshToken = context.Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshToken))
                throw new UnauthorizedAccessException("Refresh token no encontrado");

            var userId = await token.GetUserIdFromTokenAsync(refreshToken);
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("Refresh token inválido o expirado");

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                throw new UnauthorizedAccessException("Usuario no encontrado");

            var usuario = user.Adapt<User>();

            await token.InvalidateTokenAsync(refreshToken);

            var tokens = await GenerateTokenAsync(usuario);

            return (tokens.AccessToken, tokens.RefreshToken, usuario);
        }

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[62];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public void RemoveAuthCookies()
        {
            var context = httpContextAccessor.HttpContext!;

            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            context.Response.Cookies.Append("access_token", "", accessCookieOptions);
            context.Response.Cookies.Append("refresh_token", "", refreshCookieOptions);
        }
        private void SetAuthCookies(string accessToken, string refreshToken)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var context = httpContextAccessor.HttpContext!;

            var accessCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationMinutes"]))
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["RefreshTokenExpirationHours"]))
            };

            context.Response.Cookies.Append("access_token", accessToken, accessCookieOptions);
            context.Response.Cookies.Append("refresh_token", refreshToken, refreshCookieOptions);
        }
    }
}
