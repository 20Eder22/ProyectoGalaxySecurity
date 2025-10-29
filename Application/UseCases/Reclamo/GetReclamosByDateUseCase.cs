using Domain;

namespace Application
{
    public class GetReclamosByDateUseCase(IReclamoRepository reclamoRepository) : IGetReclamosByDateUseCase
    {
        public async Task<List<GetReclamoResponse>> ExecuteAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var reclamos = await reclamoRepository.FindByDateRangeAsync(fechaInicio, fechaFin);

            if (reclamos == null || !reclamos.Any())
                return [];

            return reclamos.Select(r => new GetReclamoResponse
            {
                Id = r.Id,
                Codigo = r.Codigo,
                Descripcion = r.Descripcion,
                Fecha = r.Fecha
            }).ToList();
        }
    }
}