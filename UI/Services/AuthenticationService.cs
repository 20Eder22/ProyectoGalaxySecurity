using UI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

namespace UI.Services
{
    public class AuthenticationService(HttpClient httpClient) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _claimsPrincipal = new(new ClaimsIdentity());

        public async Task Logout()
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Auth/Logout");
            request.SetBrowserRequestOption("credentials", "include");

            await httpClient.SendAsync(request);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void Authenticate()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/api/Auth/Me");
                request.SetBrowserRequestOption("credentials", "include");

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    return new AuthenticationState(_claimsPrincipal);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<ClaimDto>>>(options);

                if(result?.IsSuccess == false || result!.Result is null)
                    return new AuthenticationState(_claimsPrincipal);

                var claims = result.Result
                    .Select(c => new Claim(c.Type, c.Value))
                    .ToList();

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "JWT"));
                return new AuthenticationState(principal);
            }
            catch
            {
                return new AuthenticationState(_claimsPrincipal);
            }
        }
    }
}
