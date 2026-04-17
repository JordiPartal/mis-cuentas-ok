using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface.Service;

public interface ITransaccionService
{
    public void ObtenerTransaccion();
    public void AgregarTransaccion();
    public Task<List<Transaccion>> ObtenerTransaccionAsync(int? mes, int? ano);
}