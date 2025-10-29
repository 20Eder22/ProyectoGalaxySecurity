using Domain;
using Mapster;
using System.Text.Json;

namespace Application
{
    public class RedeemUseCase(ICaptchaService captchaService) : IRedeemUseCase
    {
        public async Task<JsonElement> ExecuteAsync(RedeemRequest request)
        {
            return await captchaService.RedeemAsync(request.Adapt<Redeem>());
        }
    }
}