using System.Globalization;
using MisCuentas.Domain.Interface;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Tmp.MenuCommand;

public class SumatorioCommand : IMenuCommand
{
    private readonly ISumatorioService _sumatorioService;
    public string Nombre => "Sumatorio";
    
    public SumatorioCommand(ISumatorioService sumatorioService)
    {
        _sumatorioService = sumatorioService;
    }
    
    public void Ejecutar()
    {
        int? mes = Validacion.LeerEnteroOpcional("Qué mes: ");
        int? ano = Validacion.LeerEnteroOpcional("Qué año: ");
        int? concepto = Validacion.LeerInput("Filtrar por categoria: ");

        if (mes > 0 && ano > 0)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(mes.Value).ToUpper();  
            var anoTexto = ano.ToString();
            
            Console.WriteLine();
            Console.WriteLine($"Sumatorio durante el mes de {mesTexto} año {anoTexto}");
            Console.WriteLine();
        }
        else if (mes > 0)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(mes.Value).ToUpper();
            ano = DateTime.Now.Year;
            
            Console.WriteLine();
            Console.WriteLine($"Sumatorio durante el mes de {mesTexto} del año en curso");
            Console.WriteLine();
        }
        else if (ano > 0)
        {
            var anoTexto = ano.ToString(); 
            
            Console.WriteLine();
            Console.WriteLine($"Sumatorio durante el año {anoTexto}");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Sumatorio general");
            Console.WriteLine();
        } 

        var  sumatorio = _sumatorioService.ObtenerSumatorio(mes, ano, concepto);
        
        ImpresoraDeConsola.ImprimirSumatorio(sumatorio);
    }
}