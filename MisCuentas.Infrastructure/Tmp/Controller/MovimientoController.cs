using MisCuentas.Domain.Interface;

namespace MisCuentas.Infrastructure.Tmp.Controller;

public class MovimientoController
{
    private readonly IMovimientoService _movimientoService;
    
    public MovimientoController(IMovimientoService movimientoService)
    {
        _movimientoService = movimientoService;
    }

    public void MovimientosContables(int? mes, int? ano)
    {
        _movimientoService.ExportarCSV(mes, ano);
    }
}