using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Tmp.Controller;

public class TransaccionController
{
    private readonly ITransaccionService _transaccionService;
    
    public TransaccionController(ITransaccionService transaccionService)
    {
        _transaccionService = transaccionService;
    }

    public List<Transaccion> ObtenerTransaccion(int? mes, int? ano)
    {
        return _transaccionService.ObtenerTransaccion(mes, ano);
    }
}