using System.Globalization;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;

namespace MisCuenta.Infrastructure.Service;

public class BalanceService : IMenuCommand, IBalanceService
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly IValidacionService _validacionService;
    private readonly IImprimirConsolaServices _imprimirConsolaServices;
    
    public string Nombre => "Balance";
    
    public BalanceService(IBalanceRepository balanceRepository, IValidacionService validacionService, IImprimirConsolaServices imprimirConsolaServices)
    {
        _balanceRepository = balanceRepository;
        _validacionService = validacionService;
        _imprimirConsolaServices = imprimirConsolaServices;
    }

    /// <summary>
    /// Retrieves the balance information for a given month and year.
    /// </summary>
    /// <param name="mes">The month for which the balance is to be retrieved. Can be null to retrieve the balance for all months.</param>
    /// <param name="ano">The year for which the balance is to be retrieved. Can be null to retrieve the balance for all years.</param>
    /// <returns>A <see cref="Balance"/> object containing the balance details, including incomes, expenses, savings, and profits.</returns>
    public Balance ObtenerBalance(int? mes, int? ano)
    {
        return _balanceRepository.Balance(mes, ano);
    }

    /// <summary>
    /// Compares the balance details of the specified month and year with the previous month.
    /// </summary>
    /// <param name="mes">The current month for which the comparison is to be made.</param>
    /// <param name="ano">The year corresponding to the current month for the comparison.</param>
    /// <returns>A <see cref="Balance"/> object containing the percentage differences in incomes, expenses, savings, and profits between the current month and the previous month.</returns>
    public async Task<Balance> ObtenerComparativaMesActualAnterior(int mes, int ano)
    {
        decimal gananciaComparada;
        int mesAnterior, anoAnterior;
        Balance balanceMesAnterior, balanceMesActual;

        try
        {
            mesAnterior = !mes.Equals(1) ? mes - 1 : 12;
            anoAnterior = !mes.Equals(1) ? ano : ano - 1;
            balanceMesActual = await _balanceRepository.BalanceAsync(mes, ano);
            balanceMesAnterior = await _balanceRepository.BalanceAsync(mesAnterior, anoAnterior);
            
            return new()
            {
                Ingresos = ((balanceMesActual.Ingresos - balanceMesAnterior.Ingresos) / Math.Abs(balanceMesAnterior.Ingresos)) * 100,
                Gastos = ((balanceMesActual.Gastos - balanceMesAnterior.Gastos) / Math.Abs(balanceMesAnterior.Gastos)) * 100,
                Ahorro = ((balanceMesActual.Ahorro - balanceMesAnterior.Ahorro) / Math.Abs(balanceMesAnterior.Ahorro)) * 100,
                Ganancia = balanceMesActual.Ganancia - balanceMesAnterior.Ganancia,
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Ingresos = 0,
                Gastos = 0,
                Ahorro = 0,
                Ganancia = 0
            };
        }
    }

    /// <summary>
    /// Executes the command to retrieve and display balance information for a specific month and year.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to input the month and year values, retrieves the corresponding balance data,
    /// and displays it using the console printing service.
    /// </remarks>
    public void Ejecutar()
    {
        var delMes = _validacionService.ValidarNumero("Qué mes: ");
        var delAno = _validacionService.ValidarNumero("Qué año: ");
        var balance = ObtenerBalance(delMes, delAno);

        if (delMes.HasValue && delAno.HasValue)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delMes.Value).ToUpper();  
            var anoTexto = delAno.ToString();
            
            Console.WriteLine();
            Console.WriteLine($"Balance durante el mes de {mesTexto} año {anoTexto}");
            Console.WriteLine();
        } 
        else if (delMes.HasValue && !(delAno.HasValue))
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delMes.Value).ToUpper();
            
            Console.WriteLine();
            Console.WriteLine($"Balance durante el mes de {mesTexto} del año en curso");
            Console.WriteLine();
        }
        else if (!(delMes.HasValue) && delAno.HasValue)
        {
            var anoTexto = delAno.ToString();
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el año {anoTexto}");
            Console.WriteLine();    
        }
        else
        {
            Console.WriteLine();
            Console.WriteLine("Balance general");
            Console.WriteLine();
        }
        
        _imprimirConsolaServices.Balance(balance);
    }
}