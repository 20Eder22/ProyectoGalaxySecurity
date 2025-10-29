namespace UI.Models
{
    public class FiltroReclamoRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class ReclamoResponse
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Codigo { get; set; } = string.Empty;
    }

}
