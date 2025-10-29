using Domain;

namespace Application
{
    public class GetReclamoByCodeUseCase(IReclamoRepository reclamoRepository) : IGetReclamoByCodeUseCase
    {
        public async Task<GetReclamoResponse> ExecuteAsync(string code)
        {
            var reclamo = await reclamoRepository.FindByCodeAsync(code);

            if (reclamo is null) { 
                throw new ApplicationException($"Reclamo with code {code} not found.");
            }

            return new GetReclamoResponse
            {
                Id = reclamo.Id,
                Codigo = reclamo.Codigo,
                Descripcion = reclamo.Descripcion,
                Fecha = reclamo.Fecha
            };
        }

    }
}