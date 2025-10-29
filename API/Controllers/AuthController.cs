using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        ICreateUserUseCase useCase,
        ILoginUseCase useLogin,
        IRefreshTokenUseCase useRefresh,
        IRemoveCookiesUseCase useRemoveCookies)
        : ControllerBase
    {
        [HttpPost("CreateUser")]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var result = await useCase.ExecuteAsync(request);
            return Ok(BaseResponse<IdentityResponse>.Success(result));
        }

        [HttpPost("Login")]
        [ValidateCaptcha]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await useLogin.ExecuteAsync(request);
            return Ok(BaseResponse<string>.Success(result));
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            var result = useRemoveCookies.ExecuteAsync();
            return Ok(BaseResponse<string>.Success(result));
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh()
        {
            await useRefresh.ExecuteAsync();
            return Ok(BaseResponse<string>.Success("Token refrescado exitosamente."));
        }

        [HttpGet("Me")]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            if (HttpContext.User.Identity is not ClaimsIdentity { IsAuthenticated: true } identity) return Unauthorized();

            var claims = identity.Claims.Select(c => new { c.Type, c.Value });

            return Ok(BaseResponse<object>.Success(claims));
        }
    }
}
