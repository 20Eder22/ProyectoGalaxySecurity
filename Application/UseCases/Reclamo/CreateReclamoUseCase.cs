using Domain;

namespace Application
{
    public class CreateReclamoUseCase(IReclamoRepository reclamoRepository) : ICreateReclamoUseCase
    {
        public async Task<IdentityResponse> ExecuteAsync(CreateReclamoRequest request)
        {
            var codigo = await ObtenerCodigo();
            var user = Reclamo.Create(codigo, request.Descripcion, request.Fecha);
            var result = await reclamoRepository.CreateAsync(user);

            return new IdentityResponse
            {
                Data = user.Codigo,
                Success = result.Success,
                Errors = result.Errors
            };
        }

        private async Task<string> ObtenerCodigo() {

            var cantidadReclamo = await reclamoRepository.CountAsync();
            return (cantidadReclamo + 1).ToString("D5");

        }

    }
}
