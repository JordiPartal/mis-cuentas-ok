using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace MisCuentas.Infrastructure.Data;

public class ConexionBd
{
    MySqlConnection _CrearConexion()
    {
        var configuracion = new ConfigurationBuilder().AddUserSecrets<ConexionBd>().Build();
        return new MySqlConnection(configuracion["ConnectionString:conexion"]);
    }
    
    public MySqlConnection CrearConexion() => _CrearConexion();
}