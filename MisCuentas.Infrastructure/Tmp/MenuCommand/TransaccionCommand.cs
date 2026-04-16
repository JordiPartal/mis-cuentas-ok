using System.Globalization;
using MisCuentas.Domain.Interface;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Tmp.MenuCommand;

public class TransaccionCommand : IMenuCommand
{
    private readonly ITransaccionService _transaccionService;
    private readonly ICsvService _csvService;
    private readonly IValidacionService _validacionService;
    
    public string Nombre => "Obtener Transacciones";
    
    public TransaccionCommand(ITransaccionService transaccionService, ICsvService csvService, IValidacionService validacionService)
    {
        _transaccionService = transaccionService;
        _csvService = csvService;
        _validacionService = validacionService;
    }
    
    public void Ejecutar()
    {
        var delMes = _validacionService.ValidarNumero("Que mes: ");
        var delAno = _validacionService.ValidarNumero("Que año: ");
        
        if (delMes > 0 && delAno > 0)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delMes.Value).ToUpper();  
            var anoTexto = delAno.ToString();
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el mes de {mesTexto} año {anoTexto}");
            Console.WriteLine();
        }
        else if (delAno > 0)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delAno.Value).ToUpper();
            delAno = DateTime.Now.Year;
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el mes de {mesTexto} del año en curso");
            Console.WriteLine();
        }
        else if (delAno > 0)
        {
            var anoTexto = delAno.ToString(); 
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el año {anoTexto}");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Gastos e ingresos");
            Console.WriteLine();
        }
        
        var transacciones = _transaccionService.ObtenerTransaccion(delMes, delAno);
        _csvService.ExportarCSV(transacciones, "transacciones");
        
        ImpresoraDeConsola.ImprimirTransaccion(transacciones);
    }
}