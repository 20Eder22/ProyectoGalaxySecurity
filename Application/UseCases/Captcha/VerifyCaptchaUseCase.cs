using Domain;

namespace Application
{
    public class VerifyCaptchaUseCase(ICaptchaService captchaService) : IVerifyCaptchaUseCase
    {
        public async Task<(bool IsValid, string Message)> ExecuteAsync(string token)
        {
            var result = await captchaService.VerifyCaptchaAsync(token);

            return result.Success ? (true, "Catpcha válido.") : (false, "Catpcha inválido.");
        }
    }
}