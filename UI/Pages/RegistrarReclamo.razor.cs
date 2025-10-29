using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using System.Text.Json;
using UI.Models;

namespace UI.Pages
{
    public partial class RegistrarReclamo
    {
        [Inject] private HttpClient HttpClient { get; set; } = null!;
        [Inject] private IToastService Toast { get; set; } = null!;
        [Inject] private NavigationManager Navegador { get; set; } = null!;

        public CreateReclamoRequest ReclamoModel { get; set; } = new();

        protected override void OnInitialized()
        {
            ReclamoModel.Fecha = DateTime.Today;
        }

        private async Task OnRegistrarReclamo()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/Reclamo/Create")
                {
                    Content = JsonContent.Create(ReclamoModel)
                };

                request.SetBrowserRequestOption("credentials", "include");
                var response = await HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<IdentityResponse>>(options);

                    if (result?.Result?.Data is not null)
                        Toast.ShowSuccess($"El reclamo {result.Result.Data} se creó correctamente");
                    else
                        Toast.ShowSuccess("El reclamo se creó correctamente.");

                    Navegador.NavigateTo("/");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Toast.ShowWarning("Usted no está autorizado para realizar esta acción.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Toast.ShowError($"Error al crear el reclamo: {error}");
                }
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        private void GoToMenu() => Navegador.NavigateTo("/");
    }
}
