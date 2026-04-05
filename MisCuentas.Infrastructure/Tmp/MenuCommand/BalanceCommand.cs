using System.Globalization;
using MisCuentas.Domain.Interface;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Tmp.MenuCommand;

public class BalanceCommand : IMenuCommand
{
    private readonly IBalanceRepository _BalanceService;
    public string Nombre => "Balance";
    
    public BalanceCommand(IBalanceRepository BalanceService)
    {
        _BalanceService = BalanceService;
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
            Console.WriteLine($"Balance durante el mes de {mesTexto} año {anoTexto}");
            Console.WriteLine();
        }
        else if (mes > 0)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(mes.Value).ToUpper();
            ano = DateTime.Now.Year;
            
            Console.WriteLine();
            Console.WriteLine($"Balance durante el mes de {mesTexto} del año en curso");
            Console.WriteLine();
        }
        else if (ano > 0)
        {
            var anoTexto = ano.ToString(); 
            
            Console.WriteLine();
            Console.WriteLine($"Balance durante el año {anoTexto}");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Balance general");
            Console.WriteLine();
        }

        var balance = _BalanceService.Balance(mes, ano);
        
        ImpresoraDeConsola.ImprimirBalance(balance);
    }
}