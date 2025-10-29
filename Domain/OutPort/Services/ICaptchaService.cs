using System.Text.Json;

namespace Domain
{
    public interface ICaptchaService
    {
        Task<JsonElement> RedeemAsync(Redeem request);
        Task<JsonElement> CreateChallengeAsync();
        Task<CaptchaVerifyDpo> VerifyCaptchaAsync(string token);
    }
}