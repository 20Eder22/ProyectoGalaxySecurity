using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;
using System.Text.Json;
using UI.Models;

namespace UI.Pages
{
    public partial class ConsultarReclamo
    {
        [Inject] private HttpClient HttpClient { get; set; } = null!;
        [Inject] private IToastService Toast { get; set; } = null!;
        [Inject] private NavigationManager Navegador { get; set; } = null!;

        public FiltroReclamoRequest Filtro { get; set; } = new();
        public List<ReclamoResponse> Reclamos { get; set; } = [];
        public bool IsLoading { get; set; }

        protected override void OnInitialized()
        {
            // Fechas iniciales por defecto (últimos 7 días)
            Filtro.FechaInicio = DateTime.Today.AddDays(-7);
            Filtro.FechaFin = DateTime.Today;
        }

        private async Task BuscarReclamos()
        {
            try
            {
                IsLoading = true;

                var request = new HttpRequestMessage(HttpMethod.Post, "/api/Reclamo/Buscar")
                {
                    Content = JsonContent.Create(Filtro)
                };

                request.SetBrowserRequestOption("credentials", "include");
                var response = await HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<List<ReclamoResponse>>>(options);

                    Reclamos = result?.Result ?? new List<ReclamoResponse>();

                    if (Reclamos.Count == 0)
                        Toast.ShowInfo("No se encontraron reclamos para el rango seleccionado.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Toast.ShowError($"Error al consultar reclamos: {error}");
                }
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Ocurrió un error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void GoToMenu() => Navegador.NavigateTo("/");
    }
}
