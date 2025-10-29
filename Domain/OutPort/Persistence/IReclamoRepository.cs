
namespace Domain
{
    public interface IReclamoRepository
    {
        Task<OperationResult> CreateAsync(Reclamo reclamo);
        Task<Reclamo?> FindByCodeAsync(string codigo);
        Task<int> CountAsync();
        Task<IEnumerable<Reclamo>> FindByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin);
    }
}