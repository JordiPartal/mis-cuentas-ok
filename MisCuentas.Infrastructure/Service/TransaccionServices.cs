using System.Globalization;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;

public class TransaccionServices : IMenuCommand, ITransaccionService
{
    private readonly ExportarConfig _exportarConfig;
    private readonly ICsvService _csvService;
    private readonly ITransaccionRepository _transaccionRepository;
    private readonly IValidacionService _validacionService;
    private readonly IImprimirConsolaServices _imprimirConsolaServices;
    
    public string Nombre => "Transacciones";
    
    public TransaccionServices(ICsvService csvService, ITransaccionRepository transaccionRepository,
        IValidacionService validacionService, IImprimirConsolaServices imprimirConsolaServices, ExportarConfig exportarConfig)
    {
        _csvService = csvService;
        _transaccionRepository = transaccionRepository;
        _validacionService = validacionService;
        _imprimirConsolaServices = imprimirConsolaServices;
        _exportarConfig = exportarConfig;
    }

    /// <summary>
    /// Retrieves and displays a list of transactions for a specific month and/or year, or all transactions if no filters are provided.
    /// </summary>
    /// <remarks>
    /// This method outputs transaction details in the console based on the user's input for month and year filters.
    /// If both month and year are provided, it displays the transactions filtered by the specified month and year.
    /// If only the month is provided, it uses the current year by default for filtering.
    /// If only the year is provided, it displays transactions for the entire year.
    /// If no filters are provided, it displays all transactions without filtering.
    /// The transaction details are fetched from the repository, and the results are printed to the console.
    /// </remarks>
    public void ObtenerTransaccion()
    {
        var delMes = _validacionService.ValidarNumero("Que mes: ");
        var delAno = _validacionService.ValidarNumero("Que año: ");
        var nombre = string.IsNullOrEmpty(_exportarConfig.NombreFichero) ? "transacciones" : _exportarConfig.NombreFichero;

        if (delMes.HasValue && delAno.HasValue)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delMes.Value).ToUpper();  
            var anoTexto = delAno.ToString();
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el mes de {mesTexto} año {anoTexto}");
            Console.WriteLine();    
        }
        else if (delMes.HasValue && !(delAno.HasValue))
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delMes.Value).ToUpper();
            delAno = DateTime.Now.Year;
            
            Console.WriteLine();
            Console.WriteLine($"Gastos e ingresos en el mes de {mesTexto} del año en curso");
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
            Console.WriteLine($"Gastos e ingresos");
            Console.WriteLine();    
        }
        
        var transacciones = _transaccionRepository.ObtenerTransaccion(delMes, delAno);
        
        _imprimirConsolaServices.Transacciones(transacciones);
        
        if(_exportarConfig.Exportar) _csvService.ExportarCSV(transacciones, nombre);
    }

    /// <summary>
    /// Retrieves a list of transactions based on the specified month and year.
    /// </summary>
    /// <remarks>
    /// This method asynchronously fetches transactions from the repository by applying the filters
    /// for the month and year if provided. Transactions are returned as a list of objects for further processing.
    /// </remarks>
    /// <param name="mes">The month to filter transactions, or null to retrieve transactions for all months.</param>
    /// <param name="ano">The year to filter transactions, or null to retrieve transactions for all years.</param>
    /// <returns>A task representing the asynchronous operation. The result contains a list of filtered transactions.</returns>
    public async Task<List<Transaccion>> ObtenerTransaccionAsync(int? mes, int? ano) =>
        await _transaccionRepository.ObtenerTransaccionAsync(mes, ano);

    /// <summary>
    /// Adds a new transaction to the repository.
    /// </summary>
    /// <remarks>
    /// This method creates a new transaction by gathering user input for various details including
    /// transaction date, concept, amount, type of expense, and type of tax. The gathered input is
    /// validated using the validation service. Console utilities are used to display selection options for
    /// types and taxes. After validation is complete, the transaction is added to the repository.
    /// </remarks>
    public void AgregarTransaccion()
    {
        var transaccion = new Transaccion();

        transaccion.fechaCargo = _validacionService.ValidarFecha("Fecha: ");
        transaccion.concepto = _validacionService.ValidarTexto("Concepto: ");
        transaccion.cantidad = _validacionService.ValidarDecimal("Cantidad: ");
        
        _imprimirConsolaServices.ImprimirTipoDeGasto();
        transaccion.idtipo = _validacionService.ValidarTipo("Tipo de gasto: ");
        
        _imprimirConsolaServices.ImprimirTipoDeImpuesto();
        transaccion.idImpuesto = _validacionService.ValidarTipo("Tipo de impuesto: ");
        
        _transaccionRepository.AgregarTransaccion(transaccion);
    }

    /// <summary>
    /// Executes a menu command for transaction management with user interaction.
    /// </summary>
    /// <remarks>
    /// This method displays a menu to the user with available options for managing transactions.
    /// Based on the user's input, it either retrieves existing transactions or adds new transactions.
    /// The selected action is performed by invoking appropriate service methods.
    /// </remarks>
    public void Ejecutar()
    {
        Console.WriteLine($"A seleccionado la opción {Nombre}, que desea realizar: ");
        Console.WriteLine();
        Console.WriteLine("(1) Obtener transacciones");
        Console.WriteLine("(2) Agregar transacciones");
        Console.WriteLine();
        Console.Write("Seleccione una opción: ");

        var input = Console.ReadLine();
        
        Console.WriteLine();
        
        if (int.Parse(input.Split(" ")[0].Trim()).Equals(1) && input.Contains("-n"))
        {
            _exportarConfig.Exportar = true;
            _exportarConfig.NombreFichero = input.Split(" ").Last();
            ObtenerTransaccion();
        }
        else if (int.Parse(input.Split(" ")[0].Trim()).Equals(1) && input.Contains("-e"))
        {
            _exportarConfig.Exportar = true;
            ObtenerTransaccion();
        }
        else if (int.Parse(input.Split(" ")[0].Trim()).Equals(2))
        {
            AgregarTransaccion();
        }
        else
        { 
            ObtenerTransaccion();
        }
    }
}