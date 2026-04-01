using MisCuentas.Infrastructure.Tmp.Utils;
using MisCuentas.Domain.Interface;

namespace MisCuentas.Infrastructure.Tmp.MenuCommand;

public class MovimientoCommand : IMenuCommand
{
    private readonly IMovimientoService _movimientoService;
    
    public string Nombre => "Obtener Movimientos";
    
    public MovimientoCommand(IMovimientoService movimientoService)
    {
        _movimientoService = movimientoService;
    }

    public void Ejecutar()
    {
        int? mes = Validacion.LeerEnteroOpcional("Qué mes: ");
        int? ano = Validacion.LeerEnteroOpcional("Qué año: ");
        
        _movimientoService.ExportarCSV(mes, ano);
    }
}