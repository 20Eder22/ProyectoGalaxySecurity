using Domain;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infraestructure
{
    public class CaptchaService(HttpClient httpClient, IVaultSecretsProvider vaultSecretsProvider)
        : ICaptchaService
    {
        public async Task<JsonElement> RedeemAsync(Redeem request)
        {
            var secrets = vaultSecretsProvider.GetSecretsAsync().GetAwaiter().GetResult();
            var accessToken = secrets["AccessTokenCap"];
            var siteKey = secrets["SideKeyCap"];
            var capUrl = secrets["ApiCap"];
            var url = $"{capUrl}{siteKey}/redeem";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"Bot {accessToken}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Error al obtener validar el challenge desde CAP");

            return JsonDocument.Parse(result).RootElement;
        }

        public async Task<JsonElement> CreateChallengeAsync()
        {
            var secrets = vaultSecretsProvider.GetSecretsAsync().GetAwaiter().GetResult();
            var accessToken = secrets["AccessTokenCap"];
            var siteKey = secrets["SideKeyCap"];
            var capUrl = secrets["ApiCap"];
            var url = $"{capUrl}{siteKey}/challenge";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"Bot {accessToken}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsync(url, null);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Error al crear el challenge desde CAP");

            return JsonDocument.Parse(result).RootElement;
        }

        public async Task<CaptchaVerifyDpo> VerifyCaptchaAsync(string token)
        {
            var secrets = vaultSecretsProvider.GetSecretsAsync().GetAwaiter().GetResult();
            var accessToken = secrets["AccessTokenCap"];
            var siteKey = secrets["SideKeyCap"];
            var capUrl = secrets["ApiCap"];
            var tokenChallenge = secrets["ChallengeTokenCap"];
            var url = $"{capUrl}{siteKey}/siteverify";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"Bot {accessToken}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var payload = new
            {
                secret = tokenChallenge,
                response = token
            };

            var response = await httpClient.PostAsJsonAsync(url, payload);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException("Error al verificar el token en CAP.");

            var verifyResponse = JsonSerializer.Deserialize<CaptchaVerifyDpo>(result,
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (verifyResponse is null)
                throw new HttpRequestException("Error al obtener respuesta del servicio verify captcha");

            return verifyResponse;
        }
    }
}
