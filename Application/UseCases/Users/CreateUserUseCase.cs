using Domain;

namespace Application
{
    public class CreateUserUseCase(IUserRepository userRepository, IEmailApiService emailApiService)
        : ICreateUserUseCase
    {
        public async Task<IdentityResponse> ExecuteAsync(CreateUserRequest request)
        {
            var user = User.Create(new Guid(), request.FullName, request.UserName, request.Email, request.Password);
            var result = await userRepository.CreateUserAsync(user, request.Role);
            if (!result.Success) throw new ApplicationException(string.Join(", ", result.Errors));

            var email = new SendEmailDpo
            {
                Recipients = request.Email,
                Subject = "Welcome to Galaxy",
                Body = $"<h1>Hola {request.FullName} </h1>" +
                    $"<p>Tu cuenta fue creado exitosamente.</p> <br>" +
                    $"<b>Usuario: {request.UserName} <br>" +
                    $"Por valor haz clic sobre este enlace para confirmar tu correo electronico<br>" +
                    $"Muchas gracias, <b>Galaxy</b>"
            };

            await emailApiService.SendEmailAsync(email);

            return new IdentityResponse
            {
                Success = result.Success,
                Errors = result.Errors
            };
        }
    }
}
