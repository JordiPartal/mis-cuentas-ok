using MisCuenta.Infrastructure.Data.Repository;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Service;

namespace MisCuenta.Infrastructure.Service;

public class RentabilidadService : IMenuCommand, IRentabilidadService
{
    private readonly ExportarConfig _exportarConfig;
    private readonly ICsvService _csvService;
    private readonly IRentabilidadRepository _rentabilidadRepository;
    
    public string Nombre => "Rentabilidad";

    public RentabilidadService(ICsvService csvService, IRentabilidadRepository rentabilidadRepository,
        ExportarConfig exportarConfig)
    {
        _csvService = csvService;
        _rentabilidadRepository = rentabilidadRepository;
        _exportarConfig = exportarConfig;
    }

    /// <summary>
    /// Retrieves profitability data from the repository and exports it to a CSV file.
    /// </summary>
    /// <remarks>
    /// The method uses the configured file name in <see cref="ExportarConfig"/> for the export.
    /// If no file name is provided, a default name of "rentabilidad" is used.
    /// The exported CSV file contains profitability data retrieved from the repository.
    /// </remarks>
    public void ObtenerRentabilidadYExportarCSV()
    {
        var nombre = string.IsNullOrEmpty(_exportarConfig.NombreFichero) ? "rentabilidad" : _exportarConfig.NombreFichero;
        var rentabilidad = _rentabilidadRepository.ObtenerRentabilidad();
        
        Console.WriteLine($">> Se han exportado {rentabilidad.Count} registros");
        
        _exportarConfig.Exportar = true;
        _csvService.ExportarCSV(rentabilidad, nombre);
    }

    public void Ejecutar()
    {
        ObtenerRentabilidadYExportarCSV();
    }
}