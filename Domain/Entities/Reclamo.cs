namespace Domain
{
    public class Reclamo
    {
        public Guid Id { get; private set; }
        public string Codigo { get; private set; }
        public string Descripcion { get; private set; }
        public DateTime Fecha { get; private set; }        
        public Reclamo()
        {
        }

        public Reclamo(string codigo, string descripcion, DateTime fecha)
        {
            Id = Guid.NewGuid();
            Codigo = "R-" + codigo;
            Descripcion = descripcion;
            Fecha = fecha;
        }

        public static Reclamo Create(string codigo, string descripcion, DateTime fecha)
        {
            if (string.IsNullOrEmpty(codigo)) throw new DomainException("El código es requerido");
            if (string.IsNullOrEmpty(descripcion)) throw new DomainException("La Descripción es requerida");

            return new Reclamo(codigo, descripcion, fecha);
        }

    }
}
