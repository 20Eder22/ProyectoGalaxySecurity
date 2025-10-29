using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class CreateReclamoRequest
    {
        [Required(ErrorMessage = "El campo Descripcion es obligatorio")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        public DateTime Fecha { get; set; }
    }
}
