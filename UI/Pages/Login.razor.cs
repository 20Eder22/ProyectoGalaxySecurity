using Blazored.Toast.Services;
using UI.Models;
using UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace UI.Pages
{
    public partial class Login
    {
        [Inject] HttpClient HttpClient { get; set; } = default!;
        [Inject] AuthenticationStateProvider Auth { get; set; } = default!;
        [Inject] IToastService Toast { get; set; } = default!;
        [Inject] NavigationManager Navegador { get; set; } = default!;
        public LoginRequest UserModel { get; set; } = new();

        private static string? _lastToken;
        
        [JSInvokable("SetCaptchaToken")]
        public static void SetCaptchaToken(string token)
        {
            _lastToken = token;
            Console.WriteLine($"Token recibido por CAP: {token}");
        }

        public async Task OnLogin()
        {
            if (string.IsNullOrEmpty(_lastToken))
            {
                Toast.ShowError($"Debes confirmar que eres humano");
            }
            else
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "/api/Auth/Login")
                    {
                        Content = JsonContent.Create(UserModel)
                    };
                    request.Headers.Add("X-Captcha-Token", _lastToken);

                    request.SetBrowserRequestOption("credentials", "include");

                    var response = await HttpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        var authService = (AuthenticationService)Auth;
                        authService.Authenticate();
                        Toast.ShowSuccess("Bienvenido al Sistema de Control de Reclamos");
                        Navegador.NavigateTo("/");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Toast.ShowWarning("Usuario o contraseña incorrectos");
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        Toast.ShowError($"Hubo un error al iniciar sesión: {error}");
                    }
                }
                catch (Exception ex)
                {
                    Toast.ShowError($"Debes confirmar que eres humano");
                }
            }
        }
    }
}
