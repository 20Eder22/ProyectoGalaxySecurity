using Domain;
using System.Net.Http.Json;

namespace Infraestructure
{
    public class EmailApiService(HttpClient httpClient) : IEmailApiService
    {
        public async Task SendEmailAsync(SendEmailDpo request)
        {
            var response = await httpClient.PostAsJsonAsync("Notifications/SendEmail", request);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error sending email: {content} - {response.StatusCode}");
            }
        }
    }
}
