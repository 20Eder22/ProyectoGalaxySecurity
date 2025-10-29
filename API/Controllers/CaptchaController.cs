using Application;
using Microsoft.AspNetCore.Mvc;

namespace Galaxy.Security.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CaptchaController(IRedeemUseCase redeemUse, ICreateChallengeUseCase challengeUseCase)
        : ControllerBase
    {
        [HttpPost("redeem")]
        public async Task<IActionResult> Redeem([FromBody] RedeemRequest request)
        {
            var result = await redeemUse.ExecuteAsync(request);
            return Ok(result);
        }

        [HttpPost("challenge")]
        public async Task<IActionResult> CreateChallenge()
        {
            var result = await challengeUseCase.ExecuteAsync();
            return Ok(result);
        }
    }
}