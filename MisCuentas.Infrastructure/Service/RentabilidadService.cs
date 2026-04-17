using Csv;
using System.Data;
using MisCuentas.Domain.Interface.Service;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Service;

public class RentabilidadService : IRentabilidadService
{
    private readonly ConexionBd conexion = new ConexionBd();

    /// <summary>
    /// Exports profitability data to a CSV file. This method retrieves data from the database using
    /// a stored procedure, processes it into a predefined format, and writes it to a CSV file on the
    /// user's desktop. The exported file is placed inside an "export" folder and named using a default
    /// or globally provided filename.
    /// This method ensures that any missing or null data is handled, and it provides status feedback
    /// indicating the number of records exported. It also handles the creation of the target directory
    /// if it does not exist.
    /// </summary>
    public void ExportarCSV()
    {
        List<Rentabilidad> rentabilidads = new List<Rentabilidad>();
        var nombre = Global.nombreCSV ?? "rentabilidad";
        var carpeta = string.Join("/", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export");
        var data = new CsvExport(columnSeparator: ",", includeColumnSeparatorDefinitionPreamble: false);
        
        using var conn = conexion.CrearConexion();
        conn.Open();
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = Consulta.Rentabilidad.rentabilidad;
        cmd.CommandType = CommandType.StoredProcedure;
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            rentabilidads.Add(new Rentabilidad()
            {
                ano = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                mes = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                mesTexto = reader.IsDBNull(2) ? "N/D" : reader.GetString(2),
                ingresos = reader.IsDBNull(3) ? 0 : Convert.ToDecimal(reader.GetValue(3)),
                gastos = reader.IsDBNull(4) ? 0 : Convert.ToDecimal(reader.GetValue(4)),
                margen = reader.IsDBNull(5) ? 0 : Convert.ToDecimal(reader.GetValue(5)),
                margenPorcentaje = reader.IsDBNull(6) ? 0 : Convert.ToDecimal(reader.GetValue(6)),
                saldo = reader.IsDBNull(7) ? 0 : Convert.ToDecimal(reader.GetValue(7)),
                rentabilidad = reader.IsDBNull(8) ? 0 : Convert.ToDecimal(reader.GetValue(8))
            });
        }

        foreach (var rentabilidad in rentabilidads)
        {
            data.AddRow();
            data["ano"] = rentabilidad.ano;
            data["mes"] = rentabilidad.mes;
            data["mesTexto"] = rentabilidad.mesTexto;
            data["ingresos"] = rentabilidad.ingresos;
            data["gastos"] = rentabilidad.gastos;
            data["margen"] = rentabilidad.margen;
            data["margenPorcentaje"] = rentabilidad.margenPorcentaje;
            data["saldo"] = rentabilidad.saldo;
            data["rentabilidad"] = rentabilidad.rentabilidad;
        }
        
        Console.WriteLine();
        Console.Write($">> Se han exportado {rentabilidads.Count} registros");
        Console.WriteLine();
        
        if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
        data.ExportToFile(string.Join("/", carpeta, string.Concat(nombre, ".csv")));
        
        Global.exportar = false;
        Global.nombreCSV = string.Empty;
    }
}