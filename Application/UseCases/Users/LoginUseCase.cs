using Domain;

namespace Application
{
    public class LoginUseCase(IAuthService authService, IUserRepository userRepository) : ILoginUseCase
    {
        public async Task<string> ExecuteAsync(LoginRequest request)
        {
            var user = await userRepository.FindByUserNameAsync(request.UserName);
            if (user is null)
                throw new UnauthorizedAccessException("Usuario no encontrado.");

            var password = await userRepository.CheckPasswordAsync(user, request.Password);
            if (!password)
                throw new UnauthorizedAccessException("Contraseña incorrecta.");

            var token = await authService.GenerateTokenAsync(user);

            return "Sesión iniciada correctamente.";
        }
    }
}
