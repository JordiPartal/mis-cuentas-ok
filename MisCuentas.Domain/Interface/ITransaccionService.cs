using MisCuentas.Domain.Models;

namespace MisCuentas.Domain.Interface;

public interface ITransaccionService
{
    public List<Transaccion> ObtenerTransaccion(int? mes, int? ano);
    public Task<List<Transaccion>> ObtenerTransaccionAsync(int? mes, int? ano);
    public void AgregarTransaccion(Transaccion transaccion);
    public void ExportarCSV(List<Transaccion> transacciones);
}