using Domain;
using System.Text.Json;

namespace Application
{
    public class CreateChallengeUseCase(ICaptchaService captchaService) : ICreateChallengeUseCase
    {
        public async Task<JsonElement> ExecuteAsync()
        {
            return await captchaService.CreateChallengeAsync();
        }
    }
}