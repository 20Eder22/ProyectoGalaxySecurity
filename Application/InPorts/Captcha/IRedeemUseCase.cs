using System.Text.Json;

namespace Application
{
    public interface IRedeemUseCase
    {
        Task<JsonElement> ExecuteAsync(RedeemRequest request);
    }
}
