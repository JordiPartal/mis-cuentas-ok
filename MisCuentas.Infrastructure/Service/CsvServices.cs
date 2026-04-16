using Csv;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;

namespace MisCuentas.Infrastructure.Service;

public class CsvServices : ICsvService
{
    private readonly ExportarConfig _config;

    public CsvServices(ExportarConfig config) => _config = config;

    public void ExportarCSV<T>(List<T> data, string nombreArchivo)
    {
        var nombre = _config.NombreFichero ?? nombreArchivo;
        var carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export");
        var datos = new CsvExport(columnSeparator : ",", includeColumnSeparatorDefinitionPreamble : false);

        foreach (var item in data)
        {
            datos.AddRow();
            var propiedades = typeof(T).GetProperties();
            foreach (var propiedad in propiedades)
            {
                datos[propiedad.Name] = propiedad.GetValue(item);
            }
        }
        
        if(!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
        datos.ExportToFile(string.Join("/", carpeta, string.Concat(nombre, ".csv")));
        
        _config.Exportar = false;
        _config.NombreFichero = string.Empty;
    }
}