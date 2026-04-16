using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;

public class TransaccionServices : ITransaccionService
{
    private readonly ICsvService _csvService;
    private readonly ITransaccionRepository _transaccionService;
    private readonly IValidacionService _validacionService;
    public string Nombre => "Obtener Transacciones";
    
    public TransaccionServices(ITransaccionRepository transaccionService) => _transaccionService = transaccionService;
    
    public List<Transaccion> ObtenerTransaccion(int? mes, int? ano) => _transaccionService.ObtenerTransaccion(mes, ano);
    
    public async Task<List<Transaccion>> ObtenerTransaccionAsync(int? mes, int? ano) => await _transaccionService.ObtenerTransaccionAsync(mes, ano);

    public void AgregarTransaccion(Transaccion transaccion) => _transaccionService.AgregarTransaccion(transaccion);
}