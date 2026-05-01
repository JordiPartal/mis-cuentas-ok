using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;

namespace MisCuenta.Infrastructure.Service;

public class MovimientoService : IMenuCommand, IMovimientoService
{
    private readonly ICsvService _csvService;
    private readonly IValidacionService _validacionService;
    private readonly ExportarConfig _exportarConfig;
    private readonly IMovimientoRepository _movimientoRepository;
    
    public string Nombre => "Obtener Movimientos";
    
    public MovimientoService(ICsvService csvService, IMovimientoRepository movimientoRepository,
        IValidacionService validacionService, ExportarConfig exportarConfig)
    {
        _csvService = csvService;
        _movimientoRepository = movimientoRepository;
        _validacionService = validacionService;
        _exportarConfig = exportarConfig;
    }

    /// <summary>
    /// Retrieves and exports a list of financial movements for a specified month and year.
    /// Prompts the user for the desired month and year, fetches the corresponding data from the repository,
    /// and exports it to a CSV file using the specified configuration.
    /// </summary>
    /// <remarks>
    /// This method interacts with various services to validate user input, fetch data from the repository,
    /// and handle CSV export configurations. It provides feedback to the console about the number of records exported.
    /// </remarks>
    public void ObtenerMovimientos()
    {
        int? delMes = _validacionService.ValidarNumero("Qué mes: ");
        int? delAno = _validacionService.ValidarNumero("Qué año: ");
        var nombre = string.IsNullOrEmpty(_exportarConfig.NombreFichero) ? "movimientos" : _exportarConfig.NombreFichero;

        var movimientos = _movimientoRepository.ObtenerMovimientos(delMes, delAno);
        
        _exportarConfig.Exportar = true;
        _csvService.ExportarCSV(movimientos, nombre);
        
        Console.WriteLine();
        Console.WriteLine($">> Se han exportado {movimientos.Count} registros");
        Console.WriteLine();
        
        _exportarConfig.Exportar = false;
        _exportarConfig.NombreFichero = string.Empty;
    }

    /// <summary>
    /// Executes the main operations related to "Movimientos" by invoking the process of retrieving
    /// financial movements and exporting them to a CSV file.
    /// </summary>
    /// <remarks>
    /// This method serves as the entry point for the "Movimientos" operations, calling the functionality
    /// that validates user input for month and year, retrieves the related financial data, handles export
    /// configurations, and exports the data as a CSV file.
    /// </remarks>
    public void Ejecutar()
    {
        ObtenerMovimientos();
    }
}