using MySql.Data.MySqlClient;

namespace MisCuentas.Domain.Interface;

public interface IGestorDeErroresService
{
    public void GestionarExcepcionesMySQL(MySqlException exception);
}