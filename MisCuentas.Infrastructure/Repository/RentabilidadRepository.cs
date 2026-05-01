using System.Data;
using MisCuentas.Domain.Interface.Repository;
using MisCuentas.Domain.Models;
using MisCuentas.Domain.Enums;
using MisCuentas.Infrastructure.Data;

namespace MisCuenta.Infrastructure.Repository;

public class RentabilidadRepository : IRentabilidadRepository
{
    private readonly ConexionBd _conexion;
    
    public RentabilidadRepository(ConexionBd conexion) => _conexion = conexion;

    /// <summary>
    /// Retrieves a list of profitability data from the database.
    /// </summary>
    /// <returns>
    /// A list of <c>Rentabilidad</c> objects containing profitability data such as year, month, revenue, expenses, margin, and other related financial metrics.
    /// </returns>
    public List<Rentabilidad> ObtenerRentabilidad()
    {
        var rentabilidad = new List<Rentabilidad>();
        using var conn = _conexion.CrearConexion();
        conn.Open();

        using var cmd = conn.CreateCommand();
        cmd.CommandText = SpRentabilidad.SP_Rentabilidad.ToString();
        cmd.CommandType = CommandType.StoredProcedure;

        using var lector = cmd.ExecuteReader();
        while (lector.Read())
        {
            rentabilidad.Add(new Rentabilidad()
            {
                Ano = lector.IsDBNull(0) ? 0 : lector.GetInt32(0),
                Mes = lector.IsDBNull(1) ? 0 : lector.GetInt32(1),
                MesTexto = lector.IsDBNull(2) ? "N/D" : lector.GetString(2),
                Ingresos = lector.IsDBNull(3) ? 0 : Convert.ToDecimal(lector.GetValue(3)),
                Gastos = lector.IsDBNull(4) ? 0 : Convert.ToDecimal(lector.GetValue(4)),
                Margen = lector.IsDBNull(5) ? 0 : Convert.ToDecimal(lector.GetValue(5)),
                MargenPorcentaje = lector.IsDBNull(6) ? 0 : Convert.ToDecimal(lector.GetValue(6)),
                Saldo = lector.IsDBNull(7) ? 0 : Convert.ToDecimal(lector.GetValue(7)),
                SuRentabilidad = lector.IsDBNull(8) ? 0 : Convert.ToDecimal(lector.GetValue(8))
            });
        }
        
        return rentabilidad;
    }
}