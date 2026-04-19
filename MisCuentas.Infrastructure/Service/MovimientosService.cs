using Csv;
using System.Data;
using MisCuentas.Domain.Interface.Service;
using MySql.Data.MySqlClient;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Service;

public class MovimientoService : IMovimientoService
{
    private readonly ConexionBd conexion = new ConexionBd();
    private readonly ExportarConfig _config;
    
    public MovimientoService(ExportarConfig config) => _config = config;

    /// <summary>
    /// Exports movements data to a CSV file for a specific month and year.
    /// </summary>
    /// <param name="mes">The month for which movements should be exported. Can be null.</param>
    /// <param name="ano">The year for which movements should be exported. Can be null.</param>
    public void ExportarCSV(int? mes, int? ano)
    {
        List<Movimiento> movimientos = new List<Movimiento>();
        var nombre = _config.NombreFichero ?? "contabilidad";
        var carpeta = string.Join("/", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export");
        var data = new CsvExport(columnSeparator: ",", includeColumnSeparatorDefinitionPreamble: false);
        
        using var conn = conexion.CrearConexion();
        conn.Open();
        
        using var cmd = conn.CreateCommand();
        cmd.CommandText = Consulta.Movimientos.obtener;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@_mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@_ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;
        
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            movimientos.Add(new Movimiento()
                {
                    id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                    ano = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                    mes = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                    dia = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    cargo = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                    tipo = reader.IsDBNull(5) ? "N/D" : reader.GetString(5),
                    concepto = reader.IsDBNull(6) ? "N/D" : reader.GetString(6),
                    debe = reader.IsDBNull(7) ? 0 : Convert.ToDecimal(reader.GetValue(7)),
                    haber = reader.IsDBNull(8) ? 0 : Convert.ToDecimal(reader.GetValue(8)),
                    saldo = reader.IsDBNull(9) ? 0 : Convert.ToDecimal(reader.GetValue(9))
                }
            );    
        }

        foreach (var movimiento in movimientos)
        {
            data.AddRow();
            data["id"] = movimiento.id;
            data["ano"] = movimiento.ano;
            data["mes"] = movimiento.mes;
            data["dia"] = movimiento.dia;
            data["cargo"] = movimiento.cargo;
            data["tipo"] = movimiento.tipo;
            data["concepto"] = movimiento.concepto;
            data["debe"] = movimiento.debe;
            data["haber"] = movimiento.haber;
            data["saldo"] = movimiento.saldo;
        }
        
        Console.WriteLine();
        Console.Write($">> Se han exportado {movimientos.Count} registros");
        Console.WriteLine();
        
        if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
        data.ExportToFile(string.Join("/", carpeta, string.Concat(nombre, ".csv")));
        
        Global.exportar = false;
        Global.nombreCSV = string.Empty;
    }
}