using Domain;

namespace Application
{
    public class RemoveCookiesUseCase(IAuthService authService) : IRemoveCookiesUseCase
    {
        public string ExecuteAsync()
        {
            authService.RemoveAuthCookies();
            return "Sesión cerrada exitosamente";
        }
    }
}
