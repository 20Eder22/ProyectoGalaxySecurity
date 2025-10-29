using System.Text.Json;

namespace Application
{
    public interface ICreateChallengeUseCase
    {
        Task<JsonElement> ExecuteAsync();
    }
}
