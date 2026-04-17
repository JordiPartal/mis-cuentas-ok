using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Repository;

public interface ITransaccionRepository
{
    public List<Transaccion> ObtenerTransaccion(int? mes, int? ano);
    public Task<List<Transaccion>> ObtenerTransaccionAsync(int? mes, int? ano);
    
    public void AgregarTransaccion(Transaccion transaccion);
}