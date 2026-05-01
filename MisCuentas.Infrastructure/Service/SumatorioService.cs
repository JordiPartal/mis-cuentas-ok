using System.Globalization;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;

namespace MisCuenta.Infrastructure.Service;

public class SumatorioService : IMenuCommand, ISumatorioService
{
    private readonly ExportarConfig _exportarConfig;
    private readonly ICsvService _csvService;
    private readonly ISumatorioRepository _sumatorioRepository;
    private readonly IValidacionService _validacionService;
    private readonly IImprimirConsolaServices _imprimirConsolaServices;
    
    public string Nombre => "Sumatorio";

    public SumatorioService(ICsvService csvService, ISumatorioRepository sumatorioRepository,
        IValidacionService validacionService, IImprimirConsolaServices imprimirConsolaServices,
        ExportarConfig exportarConfig)
    {
        _csvService = csvService;
        _sumatorioRepository = sumatorioRepository;
        _validacionService = validacionService;
        _imprimirConsolaServices = imprimirConsolaServices;
        _exportarConfig = exportarConfig;
    }

    /// <summary>
    /// Generates a summary based on user-defined criteria such as month, year, or category.
    /// Provides filtering options and displays the summary either for the requested criteria
    /// or for a general case if no specific criteria are provided.
    /// Additionally, the method supports exporting the generated summary to a CSV file
    /// if export configuration is enabled. The name of the exported file is configurable.
    /// </summary>
    public void ObtenerSumatorio()
    {
        int? delMes = _validacionService.ValidarNumero("Qué mes: ");
        int? delAno = _validacionService.ValidarNumero("Qué año: ");
        int? concepto = _validacionService.ValidarInput("Filtrar por categoria: ");
        var nombre = string.IsNullOrEmpty(_exportarConfig.NombreFichero) ? "sumatorios" : _exportarConfig.NombreFichero;
        
        if (delMes.HasValue && delAno.HasValue)
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delMes.Value).ToUpper();  
            var anoTexto = delAno.ToString();
            
            Console.WriteLine();
            Console.WriteLine($"Sumatorio durante el mes de {mesTexto} año {anoTexto}");
            Console.WriteLine();   
        }
        else if (delMes.HasValue && !(delAno.HasValue))
        {
            var mesTexto = new CultureInfo("es-ES").DateTimeFormat.GetMonthName(delMes.Value).ToUpper();
            
            Console.WriteLine();
            Console.WriteLine($"Sumatorio durante el mes de {mesTexto} del año en curso");
            Console.WriteLine();   
        }
        else if (!(delMes.HasValue) && delAno.HasValue)
        {
            var anoTexto = delAno.ToString();
            
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
        
        var sumatorios = _sumatorioRepository.ObtenerSumatorio(delMes, delAno, concepto);
        
        _imprimirConsolaServices.Sumatorios(sumatorios);
        
        if(_exportarConfig.Exportar) _csvService.ExportarCSV(sumatorios, nombre);
        
        _exportarConfig.Exportar = false;
        _exportarConfig.NombreFichero = string.Empty;
    }

    /// <summary>
    /// Executes the process of generating and displaying summary information,
    /// as defined in the implementation's specific logic. This method integrates
    /// functionality to retrieve summarized data, present it to the user, and, if configured,
    /// export the data to a file in CSV format. It acts as the entry point to trigger the
    /// summary-related operations within the service.
    /// </summary>
    public void Ejecutar()
    {
        ObtenerSumatorio();
    }
}