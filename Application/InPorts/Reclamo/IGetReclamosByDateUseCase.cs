namespace Application
{
    public interface IGetReclamosByDateUseCase
    {
        Task<List<GetReclamoResponse>> ExecuteAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}
