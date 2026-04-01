using System.Globalization;
using MisCuentas.Domain.Interface;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Tmp.MenuCommand;

public class TransaccionCommand : IMenuCommand
{
    private readonly ITransaccionService _transaccionService;
    public string Nombre => "Obtener Transacciones";

    public TransaccionCommand(ITransaccionService transaccionService)
    {
        _transaccionService = transaccionService;
    }
    
    public void Ejecutar()
    {
        int? mes = Validacion.LeerEnteroOpcional("Qué mes: ");
        int? ano = Validacion.LeerEnteroOpcional("Qué año: ");

        if (mes > 0 && ano > 0)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(mes.Value).ToUpper();  
            var anoTexto = ano.ToString();
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el mes de {mesTexto} año {anoTexto}");
            Console.WriteLine();
        }
        else if (mes > 0)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(mes.Value).ToUpper();
            ano = DateTime.Now.Year;
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el mes de {mesTexto} del año en curso");
            Console.WriteLine();
        }
        else if (ano > 0)
        {
            var anoTexto = ano.ToString(); 
            
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
        
        var transaccionesFiltrado = _transaccionService.ObtenerTransaccion(mes, ano);
        
        ImpresoraDeConsola.ImprimirTransaccion(transaccionesFiltrado);
    }
}