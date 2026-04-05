using Csv;
using System.Data;
using MySql.Data.MySqlClient;
using MisCuentas.Infrastructure.Data;
using MisCuentas.Domain.Interface;
using MisCuentas.Domain.Models;
using MisCuentas.Infrastructure.Tmp.Utils;

namespace MisCuentas.Infrastructure.Service;

public class TransaccionServices : ITransaccionService
{
    private readonly ConexionBd conexion = new ConexionBd();
    
    /// <summary>
    /// Obtener todas las transacciones filtradas o sin filtrar
    /// </summary>
    /// <param name="mes"></param>
    /// <param name="ano"></param>
    /// <returns></returns>
    public List<Transaccion> ObtenerTransaccion(int? mes, int? ano)
    {
        var transacciones = new List<Transaccion>();

        using var conn = conexion.CrearConexion();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = Consulta.Transacciones.obtener;
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@mes", MySqlDbType.Int32).Value = mes.HasValue ? mes.Value : DBNull.Value;
        cmd.Parameters.AddWithValue("@ano", MySqlDbType.Int32).Value = ano.HasValue ? ano.Value : DBNull.Value;

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            transacciones.Add(new Transaccion
            {
                fechaCargo = reader.IsDBNull(0) ? DateTime.Now : reader.GetDateTime(0),
                tipo = reader.IsDBNull(1) ? "N/D" : reader.GetString(1),
                concepto = reader.IsDBNull(2) ? "N/D" : reader.GetString(2),
                _base = reader.IsDBNull(3) ? 0 : Convert.ToDecimal(reader.GetValue(3)),
                cuota = reader.IsDBNull(4) ? 0 :Convert.ToDecimal(reader.GetValue(4)),
                cantidad = reader.IsDBNull(5) ? 0 : Convert.ToDecimal(reader.GetValue(5))
            });
        }
        
        if(Global.exportar) ExportarCSV(transacciones);

        return transacciones;    
    }

    /// <summary>
    /// Añadir una transacción
    /// </summary>
    /// <param name="transaccion"></param>
    public void AgregarTransaccion(Transaccion transaccion)
    {
        using var conn = conexion.CrearConexion();
        conn.Open();
        
        using var cmd = conn.CreateCommand();
        
        cmd.CommandText = Consulta.Transacciones.inserta;
        cmd.CommandType = CommandType.StoredProcedure;
        
        cmd.Parameters.AddWithValue("@fecha", transaccion.fechaCargo);
        cmd.Parameters.AddWithValue("@concepto", transaccion.concepto);
        cmd.Parameters.AddWithValue("@cantidad", transaccion.cantidad);
        cmd.Parameters.AddWithValue("@categoria", transaccion.idtipo);
        cmd.Parameters.AddWithValue("@impuesto", transaccion.idImpuesto);
        
        cmd.ExecuteNonQuery();
        
        Console.WriteLine();
    }

    /// <summary>
    /// Exporta una lista de transacciones a un archivo CSV en una ubicación predefinida.
    /// </summary>
    /// <param name="transacciones">Lista de transacciones a exportar.</param>
    public void ExportarCSV(List<Transaccion> transacciones)
    {
        var nombre = Global.nombreCSV ?? "transacciones";
        var carpeta = string.Join("/", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "export");
        var data = new CsvExport(columnSeparator: ",", includeColumnSeparatorDefinitionPreamble: false);

        foreach (var transaccion in transacciones)
        {
            data.AddRow();
            data["cargo"] = transaccion.fechaCargo;
            data["tipo"] = transaccion.tipo;
            data["concepto"] = transaccion.concepto;
            data["sin impuesto"] = transaccion._base;
            data["iva/irpf"] = transaccion.cuota;
            data["total"] = transaccion.cantidad;
        }
        
        if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
        data.ExportToFile(string.Join("/", carpeta, string.Concat(nombre, ".csv")));
        
        Global.exportar = false;
        Global.nombreCSV = string.Empty;
    }
}