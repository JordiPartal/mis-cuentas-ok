using Csv;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;

public class CsvService : ICsvService
{
    private readonly ExportarConfig _config;

    public CsvService(ExportarConfig config) => _config = config;

    /// <summary>
    /// Exports a list of objects to a CSV file. The file is saved in the "export" folder
    /// on the user's desktop with the specified filename.
    /// </summary>
    /// <typeparam name="T">The type of the objects in the list to be exported.</typeparam>
    /// <param name="lista">The list of objects to export.</param>
    /// <param name="nombreArchivo">The name of the CSV file to be created. If null, a default name is used.</param>
    public void ExportarCSV<T>(List<T> lista, string nombreArchivo)
    {
        var nombre = nombreArchivo;
        var carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export");
        var datos = new CsvExport(columnSeparator : ",", includeColumnSeparatorDefinitionPreamble : false);

        if (typeof(T) == typeof(Transaccion))
        {
            foreach (var item in lista.Cast<Transaccion>())
            {
                datos.AddRow();
                datos["cargo"] = item.FechaCargo.ToString("yyyy-MM-dd");
                datos["tipo"] = item.Tipo;
                datos["concepto"] = item.Concepto;
                datos["sin impuestos"] = item.BaseImponible;
                datos["iva/irpf"] = item.Cuota;
                datos["total"] = item.Cantidad;
            }
        }
        else
        {
            foreach (var item in lista)
            {
                datos.AddRow();
                var propiedades = typeof(T).GetProperties();
                foreach (var propiedad in propiedades)
                {
                    datos[propiedad.Name] = propiedad.GetValue(item);
                }
            }
        }
        
        if(!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
        datos.ExportToFile(string.Join("/", carpeta, string.Concat(nombre, ".csv")));
        
        _config.Exportar = false;
        _config.NombreFichero = string.Empty;
    }
}