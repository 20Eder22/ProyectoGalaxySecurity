using System.ComponentModel.DataAnnotations;

namespace Application
{
    public class CreateReclamoRequest
    {
        [Required(ErrorMessage = "El campo Descripcion es obligatorio")]
        public string Descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        public DateTime Fecha { get; set; }
    }
}