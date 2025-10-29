using Application;
using Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamoController(
        ICreateReclamoUseCase createReclamoUseCase,
        IGetReclamoByCodeUseCase getReclamoByCodeUseCase,
        IGetReclamosByDateUseCase getReclamosByDateUseCase)
        : ControllerBase
    {
        [HttpPost("Create")]
        [Authorize(Roles = $"{RolesConstants.ManagerRole},{RolesConstants.AdminRole}")]
        public async Task<IActionResult> Create([FromBody] CreateReclamoRequest request)
        {
            var result = await createReclamoUseCase.ExecuteAsync(request);
            return Ok(BaseResponse<IdentityResponse>.Success(result));
        }

        [HttpGet("Get/{code}")]
        [Authorize(Roles = $"{RolesConstants.CustomerRole},{RolesConstants.AdminRole}")]
        public async Task<IActionResult> Get(string code)
        {
            var result = await getReclamoByCodeUseCase.ExecuteAsync(code);
            return Ok(BaseResponse<object>.Success(result));
        }

        [HttpPost("Buscar")]
        [Authorize(Roles = $"{RolesConstants.CustomerRole},{RolesConstants.AdminRole}")]
        public async Task<ActionResult<BaseResponse<List<GetReclamoResponse>>>> Buscar([FromBody] FiltroReclamoRequest filtro)
        {
            var reclamos = await getReclamosByDateUseCase.ExecuteAsync(filtro.FechaInicio, filtro.FechaFin);

            return Ok(new BaseResponse<List<GetReclamoResponse>>
            {
                Message = reclamos.Any()
                    ? "Reclamos obtenidos correctamente."
                    : "No se encontraron reclamos en el rango indicado.",
                Result = reclamos
            });
        }

    }
}
