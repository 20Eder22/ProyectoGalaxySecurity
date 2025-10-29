using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure
{
    public class ReclamoRepository(IdentityDbContext context) : IReclamoRepository
    {
        public async Task<OperationResult> CreateAsync(Reclamo reclamo)
        {
            await context.AddAsync(reclamo);
            await context.SaveChangesAsync();
            return OperationResult.Ok();
        }

        public async Task<Reclamo?> FindByCodeAsync(string codigo)
        {
            var reclamo = await context.Reclamos.SingleOrDefaultAsync(x => x.Codigo.Equals(codigo));
            return reclamo;
        }

        public async Task<int> CountAsync()
        {
            var cantidad = await context.Reclamos.CountAsync();
            return cantidad;
        }

        public async Task<IEnumerable<Reclamo>> FindByDateRangeAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await context.Reclamos
                .Where(r => r.Fecha >= fechaInicio && r.Fecha <= fechaFin)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }
    }
}