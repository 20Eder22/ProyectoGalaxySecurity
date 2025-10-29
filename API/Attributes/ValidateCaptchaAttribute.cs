using Application;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ValidateCaptchaAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var verifyUseCase = context.HttpContext.RequestServices.GetService<IVerifyCaptchaUseCase>();

            if (verifyUseCase is null)
            {
                throw new ArgumentNullException("El servicio de verificación de Captcha no responde.");
            }

            var tokenClient = context.HttpContext.Request.Headers["X-Captcha-Token"].FirstOrDefault();
            if (string.IsNullOrEmpty(tokenClient))
            {
                throw new ArgumentNullException("No se proporcionó el token del Captcha.");
            }

            var result = await verifyUseCase.ExecuteAsync(tokenClient);

            if (!result.IsValid)
            {
                throw new ArgumentNullException(result.Message ?? "Captcha inválido.");
            }

            await next();
        }
    }
}