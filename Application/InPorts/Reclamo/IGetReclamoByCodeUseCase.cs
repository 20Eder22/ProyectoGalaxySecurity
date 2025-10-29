namespace Application
{
    public interface IGetReclamoByCodeUseCase
    {
        Task<GetReclamoResponse> ExecuteAsync(string code);
    }
}