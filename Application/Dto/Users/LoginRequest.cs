using System.ComponentModel.DataAnnotations;

namespace Application
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El campo Usuario es obligatorio")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string Password { get; set; } = null!;
    }
}
